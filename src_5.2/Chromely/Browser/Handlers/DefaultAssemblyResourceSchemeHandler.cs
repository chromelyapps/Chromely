// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

public class DefaultAssemblyResourceSchemeHandler : CefResourceHandler
{
    protected readonly IChromelyConfiguration _config;
    protected IChromelyResource _chromelyResource;
    protected readonly IChromelyErrorHandler _chromelyErrorHandler;
    protected Regex _regex = new("[/]");

    protected FileInfo? _fileInfo;
    protected bool _completed;
    protected int _totalBytesRead;


    public DefaultAssemblyResourceSchemeHandler(IChromelyConfiguration config, IChromelyErrorHandler chromelyErrorHandler)
    {
        _config = config;
        _chromelyResource = new ChromelyResource();
        _chromelyErrorHandler = chromelyErrorHandler;
        _fileInfo = null;
    }

    [Obsolete("ProcessRequest is obsolete.")]
    protected override bool ProcessRequest(CefRequest request, CefCallback callback)
    {
        var u = new Uri(request.Url);
        var fileAbsolutePath = u.AbsolutePath;
        var file = u.Authority + fileAbsolutePath;
        if (string.IsNullOrEmpty(Path.GetFileName(file)))
        {
            file = Path.Combine(file, "index.html");
        }

        _totalBytesRead = 0;
        _chromelyResource.Content = null;
        _completed = false;

        if (ProcessAssmblyEmbeddedFile(request.Url, file, fileAbsolutePath, callback))
        {
            return true;
        }

        if (ProcessLocalFile(file, callback))
        {
            return true;
        }

        callback.Dispose();
        return false;
    }

    protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
    {
        // unknown content-length
        // no-redirect
        responseLength = -1;
        redirectUrl = string.Empty;

        try
        {
            var headers = response.GetHeaderMap();
            headers.Add("Access-Control-Allow-Origin", "*");
            response.SetHeaderMap(headers);

            response.Status = (int)_chromelyResource.StatusCode;
            response.MimeType = _chromelyResource.MimeType;
            response.StatusText = _chromelyResource.StatusText;
        }
        catch (Exception exception)
        {
            _chromelyResource = _chromelyErrorHandler.HandleError(_fileInfo, exception);
            response.Status = (int)_chromelyResource.StatusCode;
            response.MimeType = _chromelyResource.MimeType;
            response.StatusText = _chromelyResource.StatusText;
        }
    }

    [Obsolete("ReadResponse is obsolete.")]
    protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
    {
        int currBytesRead = 0;

        try
        {
            if (_completed)
            {
                bytesRead = 0;
                _totalBytesRead = 0;
                _chromelyResource.Content = null;
                return false;
            }
            else
            {
                if (_chromelyResource.Content is not null)
                {
                    var fileBytes = _chromelyResource.Content.ToArray();
                    currBytesRead = Math.Min(fileBytes.Length - _totalBytesRead, bytesToRead);
                    response.Write(fileBytes, _totalBytesRead, currBytesRead);
                    _totalBytesRead += currBytesRead;

                    if (_totalBytesRead >= fileBytes.Length)
                    {
                        _completed = true;
                    }
                }
                else
                {
                    bytesRead = 0;
                    _completed = true;
                }
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        bytesRead = currBytesRead;
        return true;
    }

    protected override void Cancel()
    {
    }

    protected override bool Open(CefRequest request, out bool handleRequest, CefCallback callback)
    {
        handleRequest = false;
        return false;
    }

    protected override bool Skip(long bytesToSkip, out long bytesSkipped, CefResourceSkipCallback callback)
    {
        bytesSkipped = 0;
        return true;
    }

    protected override bool Read(IntPtr dataOut, int bytesToRead, out int bytesRead, CefResourceReadCallback callback)
    {
        bytesRead = -1;
        return false;
    }

    private bool ProcessLocalFile(string file, CefCallback callback)
    {
        _fileInfo = new FileInfo(file);

        // Check if file exists 
        if (!_fileInfo.Exists)
        {
            _chromelyResource = _chromelyErrorHandler.HandleError(_fileInfo);
            callback.Continue();
        }
        // Check if file exists but empty
        else if (_fileInfo.Length == 0)
        {
            _chromelyResource = _chromelyErrorHandler.HandleError(_fileInfo);
            callback.Continue();
        }
        else
        {
            Task.Run(() =>
            {
                using (callback)
                {
                    try
                    {
                        var fileBytes = File.ReadAllBytes(file);
                        _chromelyResource.Content = new MemoryStream(fileBytes);

                        string extension = Path.GetExtension(file);
                        _chromelyResource.MimeType = MimeMapper.GetMimeType(extension);
                        _chromelyResource.StatusCode = ResourceConstants.StatusOK;
                        _chromelyResource.StatusText = ResourceConstants.StatusOKText;
                    }
                    catch (Exception exception)
                    {
                        _chromelyResource = _chromelyErrorHandler.HandleError(_fileInfo, exception);
                    }
                    finally
                    {
                        callback.Continue();
                    }
                }
            });

            return true;
        }

        return false;
    }

    private bool ProcessAssmblyEmbeddedFile(string url, string file, string fileAbsolutePath, CefCallback callback)
    {
        var urlScheme = _config?.UrlSchemes?.GetScheme(url, UrlSchemeType.AssemblyResource);
        AssemblyOptions? option = urlScheme?.AssemblyOptions;
        if (option is null || option.TargetAssembly is null)
        {
            return false;
        }

        var manifestName = string.Join(".", option.DefaultNamespace, option.RootFolder, _regex.Replace(fileAbsolutePath, ".")).Replace("..", ".").Replace("..", ".");
        Stream? stream = option.TargetAssembly.GetManifestResourceStream(manifestName);

        // Check if file exists 
        if (stream is null)
        {
            _chromelyResource = _chromelyErrorHandler.HandleError(stream);
            callback.Continue();
        }
        // Check if file exists but empty
        else if (stream.Length == 0)
        {
            _chromelyResource = _chromelyErrorHandler.HandleError(stream);
            stream.Dispose();

            callback.Continue();
        }
        else
        {
            Task.Run(() =>
            {
                using (callback)
                {
                    try
                    {
                        var fileBytes = new byte[stream.Length];
                        stream.Read(fileBytes, 0, (int)stream.Length);
                        stream.Flush();
                        stream.Dispose();

                        _chromelyResource.Content = new MemoryStream(fileBytes);
                        string extension = Path.GetExtension(file);
                        _chromelyResource.MimeType = MimeMapper.GetMimeType(extension);
                        _chromelyResource.StatusCode = ResourceConstants.StatusOK;
                        _chromelyResource.StatusText = ResourceConstants.StatusOKText;
                    }
                    catch (Exception exception)
                    {
                        _chromelyResource = _chromelyErrorHandler.HandleError(_fileInfo, exception);
                    }
                    finally
                    {
                        callback.Continue();
                    }
                }
            });

            return true;
        }

        return false;
    }
}