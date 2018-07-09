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
    using System.Threading.Tasks;
    using Chromely.CefGlue.Winapi.Browser.ServerHandlers;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;

    /// <summary>
    /// The demo controller.
    /// </summary>
    [ControllerProperty(Name = "WebsocketDemoController", Route = "websocketmanager")]
    public class WebsocketDemoController : ChromelyController
    {
        /// <summary>
        /// The m seconds delay.
        /// </summary>
        private readonly int mSecondsDelay;

        /// <summary>
        /// The m receive from server.
        /// </summary>
        private bool mReceiveFromServer;

        /// <summary>
        /// The m receiver connection id.
        /// </summary>
        private int mReceiverConnectionId;

        /// <summary>
        /// The m server sent messages.
        /// </summary>
        private List<string> mServerSentMessages;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsocketDemoController"/> class.
        /// </summary>
        public WebsocketDemoController()
        {
            this.mReceiveFromServer = false;
            this.mSecondsDelay = 5;
            this.mReceiverConnectionId = 0;

            this.mServerSentMessages = new List<string>();
            this.mServerSentMessages.Add("https://github.com/mattkol/Chromely");
            this.mServerSentMessages.Add("Chromely Webscocket demo");
            this.mServerSentMessages.Add("Build HTML5 desktop apps with Chromely");
            this.mServerSentMessages.Add("Real-time app dev with Chromely");
            this.mServerSentMessages.Add("For more info see - https://github.com/mattkol/Chromely/wiki/Real-time-with-Websocket");

            this.RegisterGetRequest("/websocketmanager/start", this.StartServer);
            this.RegisterGetRequest("/websocketmanager/stop", this.StopServer);
            this.RegisterGetRequest("/websocketmanager/status", this.CheckStatus);
            this.RegisterPostRequest("/receivefromserver/start", this.StartReceivingFromServer);
            this.RegisterPostRequest("/receivefromserver/stop", this.StopReceivingFromServer);
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

        /// <summary>
        /// The start receiving from server.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private ChromelyResponse StartReceivingFromServer(ChromelyRequest request)
        {
            string clientname = request.PostData?.ToString() ?? string.Empty;
            this.mReceiverConnectionId = ConnectionNameMapper.GetConnectionId(clientname);
            if (!this.mReceiveFromServer)
            {
                this.mReceiveFromServer = true;
                this.SendMessagesToClient();
            }

            ChromelyResponse response = new ChromelyResponse(request.Id);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = "OK";
            return response;
        }

        /// <summary>
        /// The stop receiving from server.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private ChromelyResponse StopReceivingFromServer(ChromelyRequest request)
        {
            this.mReceiveFromServer = false;

            ChromelyResponse response = new ChromelyResponse(request.Id);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = "OK";
            return response;
        }

        /// <summary>
        /// The send messages to client.
        /// </summary>
        private void SendMessagesToClient()
        {
            Task.Run(() =>
                {
                    try
                    {
                        int index = 0;
                        while (this.mReceiveFromServer)
                        {
                            string info = this.mServerSentMessages[index];
                            string data = $"{DateTime.Now}: {info}.";
                            WebsocketMessageSender.Send(this.mReceiverConnectionId, data);
                            System.Threading.Thread.Sleep(this.mSecondsDelay * 1000);
                            index++;
                            index = (index >= this.mServerSentMessages.Count) ? 0 : index;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception);
                    }
                });
        }
    }
}
