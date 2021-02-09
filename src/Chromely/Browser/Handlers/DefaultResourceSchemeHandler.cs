// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Chromely.Core.Logging;
using Chromely.Core.Network;
using Microsoft.Extensions.Logging;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    public class DefaultResourceSchemeHandler : CefResourceHandler
    {
        protected IChromelyResource _chromelyResource;
        protected IChromelyErrorHandler _chromelyErrorHandler;

        protected FileInfo _fileInfo;
        protected bool _completed;
        protected int _totalBytesRead;

        public DefaultResourceSchemeHandler(IChromelyErrorHandler chromelyErrorHandler)
        {
            _chromelyResource = new ChromelyResource();
            _chromelyErrorHandler = chromelyErrorHandler;
            _fileInfo = null;
        }

        [Obsolete]
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            var u = new Uri(request.Url);
            var file = u.Authority + u.AbsolutePath;

            _totalBytesRead = 0;
            _chromelyResource.Content = null;
            _completed = false;

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
                            _chromelyResource =_chromelyErrorHandler.HandleError(_fileInfo, exception);
                        }
                        finally
                        {
                            callback.Continue();
                        }
                    }
                });

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
                    _chromelyResource.Content = null;
                    return false;
                }
                else
                {
                    if (_chromelyResource.Content != null)
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
    }
}
