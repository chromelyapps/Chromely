// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyWebsocketHandler.cs" company="Chromely">
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

namespace Chromely.Core
{
    using System;
    using Chromely.Core.RestfulService;

    /// <summary>
    /// The ChromelyWebsocketHandler interface.
    /// </summary>
    public interface IChromelyWebsocketHandler
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
        void OnMessage(int connectionId, IntPtr data, long dataSize);

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
        void Send(int connectionId, IntPtr data, long dataSize);

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        void Send(int connectionId, string data);

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        void Send(int connectionId, ChromelyResponse response);

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
        void Broadcast(int connectionId, IntPtr data, long dataSize);

        /// <summary>
        /// The broadcast.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        void Broadcast(int connectionId, string data);

        /// <summary>
        /// The broadcast.
        /// </summary>
        /// <param name="connectionId">
        /// The connection id.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        void Broadcast(int connectionId, ChromelyResponse response);
    }
}
