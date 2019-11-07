// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueResourceSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Chromely.Core.Helpers;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    public class CefGlueResourceSchemeHandler : CefResourceHandler
    {
        private byte[] _fileBytes;
        private string _mime;
        private bool _completed;
        private int _totalBytesRead;

        [Obsolete]
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            var u = new Uri(request.Url);
            var file = u.Authority + u.AbsolutePath;

            _totalBytesRead = 0;
            _fileBytes = null;
            _completed = false;

            // Check if file exists and not empty
            var fileInfo = new FileInfo(file);
            if ((fileInfo.Exists) && fileInfo.Length > 0)
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
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.Log.Error(exception);
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

                response.Status = (int)HttpStatusCode.OK;
                response.MimeType = _mime;
                response.StatusText = "OK";
            }
            catch (Exception exception)
            {
                response.Status = (int)HttpStatusCode.BadRequest; 
                response.MimeType = "text/plain";
                response.StatusText = "Resource loading error.";

                Logger.Instance.Log.Error(exception);
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
                Logger.Instance.Log.Error(exception);
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
