// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueWebsocketHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Chromely.CefGlue.RestfulService;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using LitJson;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.ServerHandlers
{
    /// <summary>
    /// The CefGlue websocket handler.
    /// </summary>
    public class CefGlueWebsocketHandler : IChromelyWebsocketHandler
    {
        private readonly CefServer _cefServer;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="cefServer"></param>
        public CefGlueWebsocketHandler(CefServer cefServer)
        {
            _cefServer = cefServer;
        }

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
                                this._cefServer.Send(connectionId, requestInfo.Data);
                                break;
                            case MessageType.TargetRecepient:
                                this._cefServer.Send(requestInfo.TargetConnectionId, requestInfo.Data);
                                break;
                            case MessageType.Broadcast:
                                this._cefServer.Broadcast(connectionId, requestInfo.Data);
                                break;
                            case MessageType.ControllerAction:
                                var jsonData = JsonMapper.ToObject<JsonData>(requestString);
                                var request = new ChromelyRequest(jsonData);
                                var response = RequestTaskRunner.Run(request);
                                this._cefServer.Send(connectionId, response);
                                break;
                            default:
                                this._cefServer.Send(connectionId, requestInfo.Data);
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
            this._cefServer.Send(connectionId, data, dataSize);
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
            this._cefServer.Send(connectionId, data);
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
            this._cefServer.Send(connectionId, response);
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
            this._cefServer.Broadcast(connectionId, data, dataSize);
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
            this._cefServer.Broadcast(connectionId, data);
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
            this._cefServer.Broadcast(connectionId, response);
        }
    }
}
