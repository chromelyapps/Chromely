// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebsocketServerRunner.cs" company="Chromely">
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

namespace Chromely.CefGlue.Winapi
{
    using System;

    using Chromely.CefGlue.Winapi.Browser.ServerHandlers;
    using Chromely.Core;
    using Chromely.Core.Infrastructure;

    /// <summary>
    /// The websocket server runner.
    /// </summary>
    public static class WebsocketServerRunner
    {
        /// <summary>
        /// The m server handler.
        /// </summary>
        private static CefGlueServerHandler mServerHandler;

        /// <summary>
        /// Gets the server address.
        /// </summary>
        public static string Address { get; private set; }

        /// <summary>
        /// Gets the port.
        /// </summary>
        public static int Port { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is server running.
        /// </summary>
        public static bool IsServerRunning
        {
            get
            {
                if (mServerHandler == null)
                {
                    return false;
                }

                return mServerHandler.IsServerRunning;
            }
        }

        /// <summary>
        /// The start server.
        /// </summary>
        public static void StartServer()
        {
            StartServer(0);
        }

        /// <summary>
        /// The start server.
        /// </summary>
        /// <param name="port">
        /// The port.
        /// </param>
        public static void StartServer(int port)
        {
            StartServer(string.Empty, port);
        }

        /// <summary>
        /// The start server.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        public static void StartServer(string address, int port)
        {
            try
            {
                Address = address;
                Port = port;

                if (port < 8000 || port > 65535)
                {
                    string errorMessage = $"Invalid port number: {port} specified. Port number must be between 8000 and 65535";
                    Log.Error(errorMessage);
                    return;
                }

                var sockeHandler = IoC.GetInstance<IChromelyWebsocketHandler>(typeof(IChromelyWebsocketHandler).FullName)
                                   ?? new CefGlueWebsocketHandler();

                ConnectionNameMapper.Clear();
                mServerHandler = new CefGlueServerHandler(sockeHandler);
                mServerHandler.StartServer(Address, Port, OnStartServerComplete);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        /// <summary>
        /// The stop server.
        /// </summary>
        public static void StopServer()
        {
            try
            {
                if (mServerHandler == null)
                {
                    Log.Info("Cannot stop server. Server is not currently running.");
                    return;
                }

                // Stop the server. OnComplete will be executed upon completion.
                mServerHandler.StopServer(OnStopServerComplete);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        /// <summary>
        /// The on start server complete.
        /// </summary>
        private static void OnStartServerComplete()
        {
            Log.Info($"Server started on {Address} and port {Port}.");
        }

        /// <summary>
        /// The on stop server complete.
        /// </summary>
        private static void OnStopServerComplete()
        {
            if (mServerHandler != null)
            {
                Log.Info($"Server on {Address} and port {Port} stopped.");
                mServerHandler.DisposeServer();
                mServerHandler = null;
            }
        }
    }
}