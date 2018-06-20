// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebsocketMessageSender.cs" company="Chromely">
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
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    using Chromely.CefGlue.Winapi.Browser.ServerHandlers;
    using Chromely.Core.Infrastructure;
    using Chromely.Core.RestfulService;
    using LitJson;
    using Xilium.CefGlue;

    /// <summary>
    /// The frame handler extension.
    /// </summary>
    public static class WebsocketMessageSender
    {
        /// <summary>
        /// The cefserver.
        /// </summary>
        private static CefServer cefserver;

        /// <summary>
        /// Gets the browser.
        /// </summary>
        private static CefServer Server
        {
            get
            {
                if (cefserver != null)
                {
                    return cefserver;
                }

                CefServer server = IoC.GetInstance<CefServer>(typeof(CefServer).FullName);
                if (server != null)
                {
                    cefserver = server;
                }

                return server;
            }
        }

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="clientName">
        /// The client name.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        public static void Send(string clientName, ChromelyResponse response)
        {
            Task.Factory.StartNew(() =>
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

                    if (Server == null)
                    {
                        return;
                    }

                    string jsonResponse = JsonMapper.ToJson(response);
                    SendMessage(clientName, jsonResponse);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            });
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
        public static void Send(int connectionId, ChromelyResponse response)
        {
            Task.Factory.StartNew(() =>
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

                    if (Server == null)
                    {
                        return;
                    }

                    string jsonResponse = JsonMapper.ToJson(response);
                    SendMessage(connectionId, jsonResponse);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            });
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
        public static void Broadcast(int connectionId, ChromelyResponse response)
        {
            Task.Factory.StartNew(() =>
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

                    if (Server == null)
                    {
                        return;
                    }

                    string jsonResponse = JsonMapper.ToJson(response);
                    BroadcastMessage(connectionId, jsonResponse);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            });
        }

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="clientName">
        /// The client name.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public static void Send(string clientName, string data)
        {
            Task.Factory.StartNew(() =>
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

                    if (Server == null)
                    {
                        return;
                    }

                    SendMessage(clientName, data);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
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
        public static void Send(int connectionId, string data)
        {
            Task.Factory.StartNew(() =>
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

                    if (Server == null)
                    {
                        return;
                    }

                    SendMessage(connectionId, data);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            });
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
        public static void Broadcast(int connectionId, string data)
        {
            Task.Factory.StartNew(() =>
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

                    BroadcastMessage(connectionId, data);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            });
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="clientName">
        /// The client name.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        private static void SendMessage(string clientName, string data)
        {
            IntPtr outIntPtr = IntPtr.Zero;

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

                if (Server == null)
                {
                    return;
                }

                byte[] outputByte = Encoding.UTF8.GetBytes(data);

                outIntPtr = Marshal.AllocHGlobal(outputByte.Length);
                Marshal.Copy(outputByte, 0, outIntPtr, outputByte.Length);

                Server.SendWebSocketMessage(connectionId, outIntPtr, outputByte.Length);
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
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        private static void SendMessage(int connectionId, string data)
        {
            IntPtr outIntPtr = IntPtr.Zero;

            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    return;
                }

                if (Server == null)
                {
                    return;
                }

                byte[] outputByte = Encoding.UTF8.GetBytes(data);

                outIntPtr = Marshal.AllocHGlobal(outputByte.Length);
                Marshal.Copy(outputByte, 0, outIntPtr, outputByte.Length);

                Server.SendWebSocketMessage(connectionId, outIntPtr, outputByte.Length);
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
        }

        /// <summary>
        /// The broadcast message.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        private static void BroadcastMessage(int connectionId, string data)
        {
            IntPtr outIntPtr = IntPtr.Zero;

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

                if (Server == null)
                {
                    return;
                }

                byte[] outputByte = Encoding.UTF8.GetBytes(data);

                outIntPtr = Marshal.AllocHGlobal(outputByte.Length);
                Marshal.Copy(outputByte, 0, outIntPtr, outputByte.Length);

                foreach (int id in connectionIds)
                {
                    if (id != connectionId)
                    {
                        Server.SendWebSocketMessage(id, outIntPtr, outputByte.Length);
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
        }
    }
}
