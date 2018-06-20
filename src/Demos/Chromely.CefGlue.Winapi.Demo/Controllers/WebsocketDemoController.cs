// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebsocketDemoController.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once StyleCop.SA1300
namespace Chromely.CefGlue.Winapi.Demo.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Chromely.Core.RestfulService;

    /// <summary>
    /// The demo controller.
    /// </summary>
    [ControllerProperty(Name = "WebsocketDemoController", Route = "websocketmanager")]
    public class WebsocketDemoController : ChromelyController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebsocketDemoController"/> class.
        /// </summary>
        public WebsocketDemoController()
        {
            this.RegisterGetRequest("/websocketmanager/start", this.StartServer);
            this.RegisterGetRequest("/websocketmanager/stop", this.StopServer);
            this.RegisterGetRequest("/websocketmanager/status", this.CheckStatus);
        }

        /// <summary>
        /// The get movies.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private ChromelyResponse StartServer(ChromelyRequest request)
        {
            WebsocketServerRunner.StartServer(string.Empty, 8181);
            ChromelyResponse response = new ChromelyResponse(request.Id);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = true;
            return response;
        }

        /// <summary>
        /// The stop server.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private ChromelyResponse StopServer(ChromelyRequest request)
        {
            WebsocketServerRunner.StopServer();
            ChromelyResponse response = new ChromelyResponse(request.Id);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = true; 
            return response;
        }

        /// <summary>
        /// The check status.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private ChromelyResponse CheckStatus(ChromelyRequest request)
        {
            ChromelyResponse response = new ChromelyResponse(request.Id);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = WebsocketServerRunner.IsServerRunning;
            return response;
        }
    }
}
