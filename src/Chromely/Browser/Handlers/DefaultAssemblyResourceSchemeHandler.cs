// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chromely.Core.Configuration;
using Chromely.Core.Logging;
using Chromely.Core.Network;
using Microsoft.Extensions.Logging;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    public class DefaultAssemblyResourceSchemeHandler : CefResourceHandler
    {
        protected readonly IChromelyConfiguration _config;
        protected Regex _regex = new Regex("[/]");

        protected byte[] _fileBytes;
        protected string _mime;
        protected bool _completed;
        protected int _totalBytesRead;
        protected int _status;
        protected string _statusText;

        public DefaultAssemblyResourceSchemeHandler(IChromelyConfiguration config)
        {
            _config = config;
            var status = ResourceFileStatus.Ok.GetStatus();
            _status = status.Item1;
            _statusText = status.Item2;
            _mime = "text/plain";
        }

        [Obsolete]
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
            _fileBytes = null;
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
            redirectUrl = null;

            try
            {
                var headers = response.GetHeaderMap();
                headers.Add("Access-Control-Allow-Origin", "*");
                response.SetHeaderMap(headers);

                response.Status = _status;
                response.MimeType = _mime;
                response.StatusText = _statusText;
            }
            catch (Exception exception)
            {
                response.Status = (int)HttpStatusCode.BadRequest;
                response.MimeType = "text/plain";
                response.StatusText = "Resource loading error.";

                Logger.Instance.Log.LogError(exception, exception.Message);
            }
        }

        [Obsolete]
        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            int currBytesRead = 0;

            try
            {
                if (_completed)
                {
                    bytesRead = 0;
                    _totalBytesRead = 0;
                    _fileBytes = null;
                    return false;
                }
                else
                {
                    if (_fileBytes != null)
                    {
                        currBytesRead = Math.Min(_fileBytes.Length - _totalBytesRead, bytesToRead);
                        response.Write(_fileBytes, _totalBytesRead, currBytesRead);
                        _totalBytesRead += currBytesRead;

                        if (_totalBytesRead >= _fileBytes.Length)
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
                Logger.Instance.Log.LogError(exception, exception.Message);
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
            var fileInfo = new FileInfo(file);
            // Check if file exists 
            if (!fileInfo.Exists)
            {
                var status = ResourceFileStatus.FileNotFound.GetStatus();
                _status = status.Item1;
                _statusText = status.Item2;

                Logger.Instance.Log.LogWarning($"File: {file}: {_statusText}");

                callback.Continue();
            }
            // Check if file exists but empty
            else if (fileInfo.Length == 0)
            {
                var status = ResourceFileStatus.ZeroFileSize.GetStatus();
                _status = status.Item1;
                _statusText = status.Item2;

                Logger.Instance.Log.LogWarning($"File: {file}: {_statusText}");

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
                            _fileBytes = File.ReadAllBytes(file);

                            string extension = Path.GetExtension(file);
                            _mime = MimeMapper.GetMimeType(extension);
                            var status = ResourceFileStatus.FileFound.GetStatus();
                            _status = status.Item1;
                            _statusText = status.Item2;
                        }
                        catch (Exception exception)
                        {
                            var status = ResourceFileStatus.FileProcessingError.GetStatus();
                            _status = status.Item1;
                            _statusText = status.Item2;

                            Logger.Instance.Log.LogError(exception, exception.Message);
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
            var option = urlScheme.AssemblyOptions;
            if (option == null || option.TargetAssembly == null)
            {
                return false;
            }

            var manifestName = string.Join(".", option.DefaultNamespace, option.RootFolder, _regex.Replace(fileAbsolutePath, ".")).Replace("..", ".").Replace("..", ".");
            var stream = option.TargetAssembly.GetManifestResourceStream(manifestName);

            // Check if file exists 
            if (stream == null)
            {
                var status = ResourceFileStatus.FileNotFound.GetStatus();
                _status = status.Item1;
                _statusText = status.Item2;

                Logger.Instance.Log.LogWarning($"File: {file}: {_statusText}");

                callback.Continue();
            }
            // Check if file exists but empty
            else if (stream.Length == 0)
            {
                var status = ResourceFileStatus.ZeroFileSize.GetStatus();
                _status = status.Item1;
                _statusText = status.Item2;
                 stream.Dispose();

                Logger.Instance.Log.LogWarning($"File: {file}: {_statusText}");

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
                            _fileBytes = new byte[stream.Length];
                            stream.Read(_fileBytes, 0, (int)stream.Length);
                            stream.Flush();
                            stream.Dispose();
                            string extension = Path.GetExtension(file);
                            _mime = MimeMapper.GetMimeType(extension);
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.Log.LogError(exception, exception.Message);
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
}
