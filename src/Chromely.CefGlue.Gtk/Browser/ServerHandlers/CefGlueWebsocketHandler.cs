// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueWebsocketHandler.cs" company="Chromely">
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

namespace Chromely.CefGlue.Gtk.Browser.ServerHandlers
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    using Chromely.CefGlue.Gtk.RestfulService;
    using Chromely.Core;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;

    using LitJson;

    /// <summary>
    /// The CefGlue websocket handler.
    /// </summary>
    public class CefGlueWebsocketHandler : IChromelyWebsocketHandler
    {
        /// <summary>
        /// The on message.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="dataSize">
        /// The data size.
        /// </param>
        public void OnMessage(int connectionId, IntPtr data, long dataSize)
        {
            Task.Run(() =>
                {
                    IntPtr tempPtr = IntPtr.Zero;

                    try
                    {
                        var managedArray = new byte[dataSize];
                        Marshal.Copy(data, managedArray, 0, (int)dataSize);
                        tempPtr = Marshal.AllocHGlobal(managedArray.Length);
                        Marshal.Copy(managedArray, 0, tempPtr, managedArray.Length);

                        var requestString = Encoding.UTF8.GetString(managedArray);
                        var requestInfo = new RequestInfo(requestString);

                        switch (requestInfo.Type)
                        {
                            case MessageType.Echo:
                                WebsocketMessageSender.Send(connectionId, requestInfo.Data);
                                break;
                            case MessageType.TargetRecepient:
                                WebsocketMessageSender.Send(requestInfo.TargetConnectionId, requestInfo.Data);
                                break;
                            case MessageType.Broadcast:
                                WebsocketMessageSender.Broadcast(connectionId, requestInfo.Data);
                                break;
                            case MessageType.ControllerAction:
                                var jsonData = JsonMapper.ToObject<JsonData>(requestString);
                                var request = new ChromelyRequest(jsonData);
                                var response = RequestTaskRunner.Run(request);
                                WebsocketMessageSender.Send(connectionId, response);
                                break;
                            default:
                                WebsocketMessageSender.Send(connectionId, requestInfo.Data);
                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception);
                    }
                    finally
                    {
                        // Free the unmanaged memory.
                        Marshal.FreeHGlobal(tempPtr);
                    }
                });
        }

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="dataSize">
        /// The data size.
        /// </param>
        public void Send(int connectionId, IntPtr data, long dataSize)
        {
            WebsocketMessageSender.Send(connectionId, data, dataSize);
        }

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public void Send(int connectionId, string data)
        {
            WebsocketMessageSender.Send(connectionId, data);
        }

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        public void Send(int connectionId, ChromelyResponse response)
        {
            WebsocketMessageSender.Send(connectionId, response);
        }

        /// <summary>
        /// The broadcast.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="dataSize">
        /// The data size.
        /// </param>
        public void Broadcast(int connectionId, IntPtr data, long dataSize)
        {
            WebsocketMessageSender.Broadcast(connectionId, data, dataSize);
        }

        /// <summary>
        /// The broadcast.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public void Broadcast(int connectionId, string data)
        {
            WebsocketMessageSender.Broadcast(connectionId, data);
        }

        /// <summary>
        /// The broadcast.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        public void Broadcast(int connectionId, ChromelyResponse response)
        {
            WebsocketMessageSender.Broadcast(connectionId, response);
        }
    }
}
