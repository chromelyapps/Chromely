// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueServerHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.ServerHandlers
{
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
        private readonly object _lockObj = new object();

        /// <summary>
        /// The websocket handler.
        /// </summary>
        private readonly IChromelyWebsocketHandler _websocketHandler;

        /// <summary>
        /// The complete callback.
        /// </summary>
        private Action _completeCallback;

        /// <summary>
        /// The m server.
        /// </summary>
        private CefServer _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefGlueServerHandler"/> class.
        /// </summary>
        /// <param name="websocketHandler">
        /// The websocket handler.
        /// </param>
        public CefGlueServerHandler(IChromelyWebsocketHandler websocketHandler)
        {
            _websocketHandler = websocketHandler;
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
                    (a, p, c) => StartServer(a, p, c);

                PostTask(CefThreadId.UI, startServer, address, port, completecallback);

                return;
            }

            if (_server == null)
            {
                if (!(port >= 1025 && port <= 65535))
                {
                    return;
                }

                Address = string.IsNullOrWhiteSpace(address) ? DefaultServerAddress : address;
                Port = port;
                _completeCallback = completecallback;


                CefServer.Create(Address, (ushort)Port, DefaultServerBacklog, this);
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
                    (p, c) => StartServer(p, c);

                PostTask(CefThreadId.UI, startServer, port, completecallback);

                return;
            }

            if (_server == null)
            {
                if (!(port >= 1025 && port <= 65535))
                {
                    return;
                }

                Address = DefaultServerAddress;
                Port = port;
                _completeCallback = completecallback;

                CefServer.Create(Address, (ushort)Port, DefaultServerBacklog, this);
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
                    (c) => StopServer(c);

                PostTask(CefThreadId.UI, stopServer, completecallback);

                return;
            }

            if (_server != null)
            {
                _completeCallback = completecallback;
                _server.Shutdown();
            }
        }

        /// <summary>
        /// The dispose server.
        /// </summary>
        public void DisposeServer()
        {
            if (_server != null)
            {
                _server.Dispose();
                IsServerRunning = false;
                _server = null;
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
            if (_server == null)
            {
                ConnectionNameMapper.Clear();
                _server = server;
                IoC.RegisterInstance(typeof(CefServer), typeof(CefServer).FullName, _server);
                IsServerRunning = server.IsRunning;
                RunCompleteCallback(server.IsRunning);
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
            if (_server != null)
            {
                _server = null;
                IsServerRunning = false;
                RunCompleteCallback(true);
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
            lock (_lockObj)
            {
                _websocketHandler?.OnMessage(connectionId, data, dataSize);
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
                    (f) => RunCompleteCallback(f);

                PostTask(CefThreadId.UI, run, isRunning);

                return;
            }

            _completeCallback?.Invoke();
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
            /// The address.
            /// </summary>
            private readonly string _address;

            /// <summary>
            /// The port.
            /// </summary>
            private readonly int _port;

            /// <summary>
            /// The completion callback.
            /// </summary>
            private readonly Action _completionCallback;

            /// <summary>
            /// The action.
            /// </summary>
            private Action<string, int, Action> _action;

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
                _address = address;
                _action = action;
                _port = port;
                _completionCallback = completionCallback;
            }

            /// <summary>
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                _action(_address, _port, _completionCallback);
                _action = null;
            }
        }

        /// <summary>
        /// The action task.
        /// </summary>
        private class ActionTask2 : CefTask
        {
            /// <summary>
            /// The port.
            /// </summary>
            private readonly int _port;

            /// <summary>
            /// The completion callback.
            /// </summary>
            private readonly Action _completionCallback;

            /// <summary>
            /// The action.
            /// </summary>
            private Action<int, Action> _action;

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
                _action = action;
                _port = port;
                _completionCallback = completionCallback;
            }

            /// <summary>
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                _action(_port, _completionCallback);
                _action = null;
            }
        }

        /// <summary>
        /// The action task.
        /// </summary>
        private class ActionTask3 : CefTask
        {
            /// <summary>
            /// The completion callback.
            /// </summary>
            private readonly Action _completionCallback;

            /// <summary>
            /// The action.
            /// </summary>
            private Action<Action> _action;

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
                _action = action;
                _completionCallback = completionCallback;
            }

            /// <summary>
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                _action(_completionCallback);
                _action = null;
            }
        }

        /// <summary>
        /// The action task.
        /// </summary>
        private class ActionTask4 : CefTask
        {
            /// <summary>
            /// The flag.
            /// </summary>
            private readonly bool _flag;

            /// <summary>
            /// The action.
            /// </summary>
            private Action<bool> _action;

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
                _action = action;
                _flag = flag;
            }

            /// <summary>
            /// The execute.
            /// </summary>
            protected override void Execute()
            {
                _action(_flag);
                _action = null;
            }
        }
    }
}
