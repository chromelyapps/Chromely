// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExternalRequestSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core.Infrastructure;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// Loads external HTTP request resources like proxy.
    /// Omits X-Frame-Options headers and adds Access-Control-Allow-Origin: * header
    /// </summary>
    public class ExternalRequestSchemeHandler : CefGlueAsyncHandlerBase
    {
        private readonly HttpClient _httpClient;
        private HttpRequestMessage _httpRequest;
        private HttpResponseMessage _httpResponseMessage;
        private long _responseLenght;

        /// <summary>
        /// Initializes a new instance of the Chromely.CefGlue.Browser.Handlers.ExternalRequestSchemeHandler class.
        /// </summary>
        /// <param name="httpClient">HttpClient object that will be used for requestes</param>
        public ExternalRequestSchemeHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Converts http request method name into object.
        /// </summary>
        protected virtual HttpMethod GetHttpMethod(string methodName)
        {
            switch (methodName.ToUpper())
            {
                case "GET":
                    return HttpMethod.Get;
                case "PUT":
                    return HttpMethod.Put;
                case "POST":
                    return HttpMethod.Post;
                case "DELETE":
                    return HttpMethod.Delete;
                case "HEAD":
                    return HttpMethod.Head;
                case "OPTIONS":
                    return HttpMethod.Options;
                case "TRACE":
                    return HttpMethod.Trace;
            }

            throw new ArgumentException($"Unknown http method: {methodName}");
        }

        /// <summary>
        /// Builds http request message from cef request
        /// </summary>
        /// <param name="cefRequest"></param>
        protected virtual HttpRequestMessage BuildHttpRequest(CefRequest cefRequest)
        {
            var httpRequest = new HttpRequestMessage(GetHttpMethod(cefRequest.Method), cefRequest.Url);
            var cefHeaders = cefRequest.GetHeaderMap();
            foreach (var key in cefHeaders.AllKeys)
                httpRequest.Headers.TryAddWithoutValidation(key, cefHeaders.GetValues(key));

            if (cefRequest.PostData != null && cefRequest.PostData.Count > 0)
                httpRequest.Content = new StreamContent(new CefPostDataStream(cefRequest.PostData.GetElements()));

            return httpRequest;
        }

        /// <inheritdoc/>
        protected override bool PrepareRequest(CefRequest request)
        {
            _httpRequest = BuildHttpRequest(request);
            return true;
        }

        /// <inheritdoc/>
        protected override async Task<bool> LoadResourceData(CancellationToken cancellationToken)
        {
            _httpResponseMessage = await _httpClient.SendAsync(_httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return _httpResponseMessage.Content != null;
        }
        /// <inheritdoc/>
        protected override Task<Stream> GetResourceDataStream(CancellationToken cancellationToken)
        {
            return _httpResponseMessage.Content.ReadAsStreamAsync();
        }

        /// <inheritdoc/>
        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            // unknown content-length
            // no-redirect
            responseLength = -1;
            redirectUrl = null;

            try
            {
                if (_httpResponseMessage == null)
                    return;
                var headers = response.GetHeaderMap();
                headers.Clear();

                this.ProcessHeaders(_httpResponseMessage, headers);
                response.SetHeaderMap(headers);

                response.MimeType = _httpResponseMessage.Content?.Headers?.ContentType?.MediaType;
                response.Status = (int)_httpResponseMessage.StatusCode;
                response.StatusText = _httpResponseMessage.ReasonPhrase;
                responseLength = this._responseLenght = _httpResponseMessage.Content?.Headers?.ContentLength ?? -1;

                if (_httpResponseMessage.StatusCode == HttpStatusCode.MovedPermanently
                    || _httpResponseMessage.StatusCode == HttpStatusCode.Moved
                    || _httpResponseMessage.StatusCode == HttpStatusCode.Redirect
                    || _httpResponseMessage.StatusCode == HttpStatusCode.RedirectMethod
                    || _httpResponseMessage.StatusCode == HttpStatusCode.TemporaryRedirect)
                    redirectUrl = _httpResponseMessage.Headers.Location.ToString();

            }
            catch (Exception ex)
            {
                response.Error = CefErrorCode.Failed;
                Logger.Instance.Log.Error(ex, "Exception thrown while processing request");
            }
        }

        /// <inheritdoc/>
        protected virtual void ProcessHeaders(HttpResponseMessage httpResponseMessage, NameValueCollection headers)
        {
            foreach (var header in _httpResponseMessage.Headers)
            {
                if (header.Key == "X-Frame-Options")
                    continue;
                foreach (var val in header.Value)
                    headers.Add(header.Key, val);
            }

            if (_httpResponseMessage.Content != null)
            {
                foreach (var header in _httpResponseMessage.Content.Headers)
                {
                    foreach (var val in header.Value)
                        headers.Add(header.Key, val);
                }
            }
            headers["Access-Control-Allow-Origin"] = "*";
        }

        /// <inheritdoc/>
        protected override long GetDataSize()
        {
            return this._responseLenght;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            this._httpRequest?.Dispose();
            this._httpResponseMessage?.Dispose();

            this._httpRequest = null;
            this._httpResponseMessage = null;

            base.Dispose(disposing);
        }
    }
}
