// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueServerHandler.cs" company="Chromely">
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
    using Chromely.Core;
    using Chromely.Core.Infrastructure;
    using Xilium.CefGlue;

    /// <summary>
    /// The CefGlue server handler.
    /// </summary>
    public class CefGlueServerHandler : CefServerHandler
    {
        /// <summary>
        /// The default server address.
        /// </summary>
        private const string DefaultServerAddress = "127.0.0.1";

        /// <summary>
        /// The default server backlog.
        /// </summary>
        private const int DefaultServerBacklog = 10;

        /// <summary>
        /// The lock obj.
        /// </summary>
        private readonly object mlockObj = new object();

        /// <summary>
        /// The m websocket handler.
        /// </summary>
        private readonly IChromelyWebsocketHandler mWebsocketHandler;

        /// <summary>
        /// The m complete callback.
        /// </summary>
        private Action mCompleteCallback;

        /// <summary>
        /// The m server.
        /// </summary>
        private CefServer mServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueServerHandler"/> class.
        /// </summary>
        /// <param name="websocketHandler">
        /// The websocket handler.
        /// </param>
        public CefGlueServerHandler(IChromelyWebsocketHandler websocketHandler)
        {
            this.mWebsocketHandler = websocketHandler;
        }

        /// <summary>
        /// Gets the address.
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Gets the port.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is server running.
        /// </summary>
        public bool IsServerRunning { get; private set; }

        /// <summary>
        /// The start server.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="completecallback">
        /// The completecallback.
        /// </param>
        public void StartServer(string address, int port, Action completecallback)
        {

            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                Action<string, int, Action> startServer =
                    (a, p, c) => this.StartServer(a, p, c);

                this.PostTask(CefThreadId.UI, startServer, address, port, completecallback);

                return;
            }

            if (this.mServer == null)
            {
                if (!(port >= 1025 && port <= 65535))
                {
                    return;
                }

                this.Address = string.IsNullOrWhiteSpace(address) ? DefaultServerAddress : address;
                this.Port = port;
                this.mCompleteCallback = completecallback;


                CefServer.Create(this.Address, (ushort)this.Port, DefaultServerBacklog, this);
            }
        }

        /// <summary>
        /// The start server.
        /// </summary>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="completecallback">
        /// The completecallback.
        /// </param>
        public void StartServer(int port, Action completecallback)
        {

            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                Action<int, Action> startServer =
                    (p, c) => this.StartServer(p, c);

                this.PostTask(CefThreadId.UI, startServer, port, completecallback);

                return;
            }

            if (this.mServer == null)
            {
                if (!(port >= 1025 && port <= 65535))
                {
                    return;
                }

                this.Address = DefaultServerAddress;
                this.Port = port;
                this.mCompleteCallback = completecallback;

                CefServer.Create(this.Address, (ushort)this.Port, DefaultServerBacklog, this);
            }
        }

        /// <summary>
        /// The stop server.
        /// </summary>
        /// <param name="completecallback">
        /// The completecallback.
        /// </param>
        public void StopServer(Action completecallback)
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                Action<Action> stopServer =
                    (c) => this.StopServer(c);

                this.PostTask(CefThreadId.UI, stopServer, completecallback);

                return;
            }

            if (this.mServer != null)
            {
                this.mCompleteCallback = completecallback;
                this.mServer.Shutdown();
            }
        }

        /// <summary>
        /// The dispose server.
        /// </summary>
        public void DisposeServer()
        {
            if (this.mServer != null)
            {
                this.mServer.Dispose();
                this.IsServerRunning = false;
                this.mServer = null;
            }
        }

        /// <summary>
        /// The on server created.
        /// </summary>
        /// <param name="server">
        /// The server.
        /// </param>
        protected override void OnServerCreated(CefServer server)
        {
            if (this.mServer == null)
            {
                ConnectionNameMapper.Clear();
                this.mServer = server;
                IoC.RegisterInstance(typeof(CefServer), typeof(CefServer).FullName, this.mServer);
                this.IsServerRunning = server.IsRunning;
                this.RunCompleteCallback(server.IsRunning);
            }
        }

        /// <summary>
        /// The on server destroyed.
        /// </summary>
        /// <param name="server">
        /// The server.
        /// </param>
        protected override void OnServerDestroyed(CefServer server)
        {
            if (this.mServer != null)
            {
                this.mServer = null;
                this.IsServerRunning = false;
                this.RunCompleteCallback(true);
            }
        }

        /// <summary>
        /// The on client connected.
        /// </summary>
        /// <param name="server">
        /// The server.
        /// </param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        protected override void OnClientConnected(CefServer server, int connectionId)
        {
        }

        /// <summary>
        /// The on client disconnected.
        /// </summary>
        /// <param name="server">
        /// The server.
        /// </param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        protected override void OnClientDisconnected(CefServer server, int connectionId)
        {
        }

        /// <summary>
        /// The on http request.
        /// </summary>
        /// <param name="server">
        /// The server.
        /// </param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="clientAddress">
        /// The client address.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        protected override void OnHttpRequest(CefServer server, int connectionId, string clientAddress, CefRequest request)
        {
        }

        /// <summary>
        /// The on web socket connected.
        /// </summary>
        /// <param name="server">
        /// The server.
        /// </param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        protected override void OnWebSocketConnected(CefServer server, int connectionId)
        {
        }

        /// <summary>
        /// The on web socket request.
        /// </summary>
        /// <param name="server">
        /// The server.
        /// </param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="clientAddress">
        /// The client address.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="callback">
        /// The callback.
        /// </param>
        protected override void OnWebSocketRequest(
            CefServer server,
            int connectionId,
            string clientAddress,
            CefRequest request,
            CefCallback callback)
        {
            // Cache name and connection identifier.
            ConnectionNameMapper.Add(request.Url, connectionId);

            // Always accept WebSocket connections.
            callback.Continue();
        }

        /// <summary>
        /// The on web socket message.
        /// </summary>
        /// <param name="server">
        /// The server.
        /// </param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="dataSize">
        /// The data size.
        /// </param>
        protected override void OnWebSocketMessage(CefServer server, int connectionId, IntPtr data, long dataSize)
        {
            lock (this.mlockObj)
            {
                this.mWebsocketHandler?.OnMessage(connectionId, data, dataSize);
            }
        }

        /// <summary>
        /// The run complete callback.
        /// </summary>
        /// <param name="isRunning">
        /// The is running.
        /// </param>
        private void RunCompleteCallback(bool isRunning)
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                Action<bool> run =
                    (f) => this.RunCompleteCallback(f);

                this.PostTask(CefThreadId.UI, run, isRunning);

                return;
            }

            if (this.mCompleteCallback != null)
            {
                this.mCompleteCallback.Invoke();
            }
        }

        /// <summary>
        /// The post task.
        /// </summary>
        /// <param name="threadId">
        /// The thread id.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="completionCallback">
        /// The completion callback.
        /// </param>
        private void PostTask(CefThreadId threadId, Action<string, int, Action> action, string address, int port, Action completionCallback)
        {
            CefRuntime.PostTask(threadId, new ActionTask1(action, address, port, completionCallback));
        }

        /// <summary>
        /// The post task.
        /// </summary>
        /// <param name="threadId">
        /// The thread id.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="completionCallback">
        /// The completion callback.
        /// </param>
        private void PostTask(CefThreadId threadId, Action<int, Action> action, int port, Action completionCallback)
        {
            CefRuntime.PostTask(threadId, new ActionTask2(action, port, completionCallback));
        }

        /// <summary>
        /// The post task.
        /// </summary>
        /// <param name="threadId">
        /// The thread id.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="completionCallback">
        /// The completion callback.
        /// </param>
        private void PostTask(CefThreadId threadId, Action<Action> action, Action completionCallback)
        {
            CefRuntime.PostTask(threadId, new ActionTask3(action, completionCallback));
        }

        /// <summary>
        /// The post task.
        /// </summary>
        /// <param name="threadId">
        /// The thread id.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <param name="flag">
        /// The flag.
        /// </param>
        private void PostTask(CefThreadId threadId, Action<bool> action, bool flag)
        {
            CefRuntime.PostTask(threadId, new ActionTask4(action, flag));
        }

        /// <summary>
        /// The action task.
        /// </summary>
        private class ActionTask1 : CefTask
        {
            /// <summary>
            /// The m address.
            /// </summary>
            private readonly string mAddress;

            /// <summary>
            /// The m port.
            /// </summary>
            private readonly int mPort;

            /// <summary>
            /// The m completion callback.
            /// </summary>
            private readonly Action mCompletionCallback;

            /// <summary>
            /// The action.
            /// </summary>
            private Action<string, int, Action> mAction;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActionTask1"/> class.
            /// </summary>
            /// <param name="action">
            /// The action.
            /// </param>
            /// <param name="address">
            /// The address.
            /// </param>
            /// <param name="port">
            /// The port.
            /// </param>
            /// <param name="completionCallback">
            /// The completion callback.
            /// </param>
            public ActionTask1(Action<string, int, Action> action, string address, int port, Action completionCallback)
            {
                this.mAddress = address;
                this.mAction = action;
                this.mPort = port;
                this.mCompletionCallback = completionCallback;
            }

            /// <summary>
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                this.mAction(this.mAddress, this.mPort, this.mCompletionCallback);
                this.mAction = null;
            }
        }

        /// <summary>
        /// The action task.
        /// </summary>
        private class ActionTask2 : CefTask
        {
            /// <summary>
            /// The m port.
            /// </summary>
            private readonly int mPort;

            /// <summary>
            /// The m completion callback.
            /// </summary>
            private readonly Action mCompletionCallback;

            /// <summary>
            /// The action.
            /// </summary>
            private Action<int, Action> mAction;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActionTask2"/> class.
            /// </summary>
            /// <param name="action">
            /// The action.
            /// </param>
            /// <param name="port">
            /// The port.
            /// </param>
            /// <param name="completionCallback">
            /// The completion callback.
            /// </param>
            public ActionTask2(Action<int, Action> action, int port, Action completionCallback)
            {
                this.mAction = action;
                this.mPort = port;
                this.mCompletionCallback = completionCallback;
            }

            /// <summary>
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                this.mAction(this.mPort, this.mCompletionCallback);
                this.mAction = null;
            }
        }

        /// <summary>
        /// The action task.
        /// </summary>
        private class ActionTask3 : CefTask
        {
            /// <summary>
            /// The m completion callback.
            /// </summary>
            private readonly Action mCompletionCallback;

            /// <summary>
            /// The action.
            /// </summary>
            private Action<Action> mAction;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActionTask3"/> class.
            /// </summary>
            /// <param name="action">
            /// The action.
            /// </param>
            /// <param name="completionCallback">
            /// The completion callback.
            /// </param>
            public ActionTask3(Action<Action> action, Action completionCallback)
            {
                this.mAction = action;
                this.mCompletionCallback = completionCallback;
            }

            /// <summary>
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                this.mAction(this.mCompletionCallback);
                this.mAction = null;
            }
        }

        /// <summary>
        /// The action task.
        /// </summary>
        private class ActionTask4 : CefTask
        {
            /// <summary>
            /// The m flag.
            /// </summary>
            private readonly bool mFlag;

            /// <summary>
            /// The action.
            /// </summary>
            private Action<bool> mAction;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActionTask4"/> class.
            /// </summary>
            /// <param name="action">
            /// The action.
            /// </param>
            /// <param name="flag">
            /// The flag.
            /// </param>
            public ActionTask4(Action<bool> action, bool flag)
            {
                this.mAction = action;
                this.mFlag = flag;
            }

            /// <summary>
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                this.mAction(this.mFlag);
                this.mAction = null;
            }
        }
    }
}
