// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebsocketMessageSender.cs" company="Chromely Projects">
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
using Chromely.CefGlue.Browser.ServerHandlers;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;
using LitJson;
using Xilium.CefGlue;

namespace Chromely.CefGlue
{
    /// <summary>
    /// The frame handler extension.
    /// </summary>
    public static class WebsocketMessageSender
    {
        /// <summary>
        /// The lock obejct 1.
        /// </summary>
        private static readonly object ObjLock1 = new object();

        /// <summary>
        /// The lock obejct 2.
        /// </summary>
        private static readonly object ObjLock2 = new object();

        /// <summary>
        /// The lock obejct 3.
        /// </summary>
        private static readonly object ObjLock3 = new object();

        /// <summary>
        /// The lock obejct 4.
        /// </summary>
        private static readonly object ObjLock4 = new object();

        /// <summary>
        /// The lock obejct 5.
        /// </summary>
        private static readonly object ObjLock5 = new object();

        /// <summary>
        /// The lock obejct 6.
        /// </summary>
        private static readonly object ObjLock6 = new object();

        /// <summary>
        /// The lock obejct 7.
        /// </summary>
        private static readonly object ObjLock7 = new object();

        /// <summary>
        /// The lock obejct 8.
        /// </summary>
        private static readonly object ObjLock8 = new object();

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="server">The Cef server</param>
        /// <param name="clientName">
        /// The client name.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        public static void Send(this CefServer server, string clientName, ChromelyResponse response)
        {
            lock (ObjLock1)
            {
                try
                {
                    if (string.IsNullOrEmpty(clientName))
                    {
                        return;
                    }

                    if (response == null)
                    {
                        return;
                    }

                    if (server == null)
                    {
                        return;
                    }

                    var jsonResponse = JsonMapper.ToJson(response);
                    SendMessage(server, clientName, jsonResponse);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            }
        }

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="server">The Cef Server</param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        public static void Send(this CefServer server, int connectionId, ChromelyResponse response)
        {
            lock (ObjLock2)
            {
                try
                {
                    if (connectionId == 0)
                    {
                        return;
                    }

                    if (response == null)
                    {
                        return;
                    }

                    if (server == null)
                    {
                        return;
                    }

                    var jsonResponse = JsonMapper.ToJson(response);
                    SendMessage(server, connectionId, jsonResponse);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            }
        }

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="clientName">
        /// The client name.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public static void Send(this CefServer server, string clientName, string data)
        {
            lock (ObjLock3)
            {
                try
                {
                    if (string.IsNullOrEmpty(clientName))
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(data))
                    {
                        return;
                    }

                    if (server == null)
                    {
                        return;
                    }

                    SendMessage(server, clientName, data);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            }
        }

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public static void Send(this CefServer server, int connectionId, string data)
        {
            lock (ObjLock4)
            {
                try
                {
                    if (connectionId == 0)
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(data))
                    {
                        return;
                    }

                    if (server == null)
                    {
                        return;
                    }

                    SendMessage(server, connectionId, data);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            }
        }

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="dataSize">
        /// The data size.
        /// </param>
        public static void Send(this CefServer server, int connectionId, IntPtr data, long dataSize)
        {
            lock (ObjLock5)
            {
                try
                {
                    if (connectionId == 0)
                    {
                        return;
                    }

                    if (server == null)
                    {
                        return;
                    }

                    SendMessage(server, connectionId, data, dataSize);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            }
        }

        /// <summary>
        /// The broadcast.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        public static void Broadcast(this CefServer server, int connectionId, ChromelyResponse response)
        {
            lock (ObjLock6)
            {
                try
                {
                    if (connectionId == 0)
                    {
                        return;
                    }

                    if (response == null)
                    {
                        return;
                    }

                    if (server == null)
                    {
                        return;
                    }

                    var jsonResponse = JsonMapper.ToJson(response);
                    BroadcastMessage(server, connectionId, jsonResponse);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            }
        }

        /// <summary>
        /// The broadcast.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public static void Broadcast(this CefServer server, int connectionId, string data)
        {
            lock (ObjLock7)
            {
                try
                {
                    if (connectionId == 0)
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(data))
                    {
                        return;
                    }

                    BroadcastMessage(server, connectionId, data);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            }
        }

        /// <summary>
        /// The broadcast.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="dataSize">
        /// The data size.
        /// </param>
        public static void Broadcast(this CefServer server, int connectionId, IntPtr data, long dataSize)
        {
            lock (ObjLock8)
            {
                try
                {
                    if (connectionId == 0)
                    {
                        return;
                    }

                    BroadcastMessage(server, connectionId, data, dataSize);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            }
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="clientName">
        /// The client name.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        private static void SendMessage(CefServer server, string clientName, string data)
        {
            Task.Run(() =>
            {
                var outIntPtr = IntPtr.Zero;

                try
                {
                    if (string.IsNullOrEmpty(data))
                    {
                        return;
                    }

                    int connectionId = ConnectionNameMapper.GetConnectionId(clientName);
                    if (connectionId == 0)
                    {
                        return;
                    }

                    if (server == null)
                    {
                        return;
                    }

                    var outputByte = Encoding.UTF8.GetBytes(data);
                    outIntPtr = Marshal.AllocHGlobal(outputByte.Length);
                    Marshal.Copy(outputByte, 0, outIntPtr, outputByte.Length);

                    server.SendWebSocketMessage(connectionId, outIntPtr, outputByte.Length);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
                finally
                {
                    // Free the unmanaged memory.
                    Marshal.FreeHGlobal(outIntPtr);
                }
            });
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        private static void SendMessage(CefServer server, int connectionId, string data)
        {
            Task.Run(() =>
            {
                var outIntPtr = IntPtr.Zero;

                try
                {
                    if (string.IsNullOrEmpty(data))
                    {
                        return;
                    }

                    if (server == null)
                    {
                        return;
                    }

                    var outputByte = Encoding.UTF8.GetBytes(data);
                    outIntPtr = Marshal.AllocHGlobal(outputByte.Length);
                    Marshal.Copy(outputByte, 0, outIntPtr, outputByte.Length);

                    server.SendWebSocketMessage(connectionId, outIntPtr, outputByte.Length);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
                finally
                {
                    // Free the unmanaged memory.
                    Marshal.FreeHGlobal(outIntPtr);
                }
            });
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="dataSize">
        /// The data size.
        /// </param>
        private static void SendMessage(CefServer server, int connectionId, IntPtr data, long dataSize)
        {
            Task.Run(() =>
                {
                    try
                    {

                        if (server == null)
                        {
                            return;
                        }

                        server.SendWebSocketMessage(connectionId, data, dataSize);
                    }
                    catch (Exception exception)
                    {
                        Log.Error(exception);
                    }
                });
        }

        /// <summary>
        /// The broadcast message.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        private static void BroadcastMessage(CefServer server, int connectionId, string data)
        {
            Task.Run(() =>
            {
                var outIntPtr = IntPtr.Zero;
                try
                {
                    if (string.IsNullOrEmpty(data))
                    {
                        return;
                    }

                    var connectionIds = ConnectionNameMapper.ConnectionIds;

                    if (connectionIds == null)
                    {
                        return;
                    }

                    if (connectionIds.Count == 0)
                    {
                        return;
                    }

                    if ((connectionIds.Count == 1) && (connectionIds[0] == connectionId))
                    {
                        return;
                    }

                    if (server == null)
                    {
                        return;
                    }

                    var outputByte = Encoding.UTF8.GetBytes(data);
                    outIntPtr = Marshal.AllocHGlobal(outputByte.Length);
                    Marshal.Copy(outputByte, 0, outIntPtr, outputByte.Length);

                    foreach (int id in connectionIds)
                    {
                        if (id != connectionId)
                        {
                            server.SendWebSocketMessage(id, outIntPtr, outputByte.Length);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
                finally
                {
                    // Free the unmanaged memory.
                    Marshal.FreeHGlobal(outIntPtr);
                }
            });
        }

        /// <summary>
        /// The broadcast message.
        /// </summary>
        /// <param name="server">The CefServer</param>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="dataSize">
        /// The data size.
        /// </param>
        private static void BroadcastMessage(CefServer server, int connectionId, IntPtr data, long dataSize)
        {
            Task.Run(() =>
                {
                    try
                    {
                        var connectionIds = ConnectionNameMapper.ConnectionIds;
                        if (connectionIds == null)
                        {
                            return;
                        }

                        if (connectionIds.Count == 0)
                        {
                            return;
                        }

                        if ((connectionIds.Count == 1) && (connectionIds[0] == connectionId))
                        {
                            return;
                        }

                        if (server == null)
                        {
                            return;
                        }

                        foreach (int id in connectionIds)
                        {
                            if (id != connectionId)
                            {
                                server.SendWebSocketMessage(id, data, dataSize);
                            }
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
