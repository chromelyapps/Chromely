// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyResponse.cs" company="Chromely">
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

namespace Chromely.Core.RestfulService
{
    /// <summary>
    /// The chromely response.
    /// </summary>
    public class ChromelyResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyResponse"/> class.
        /// </summary>
        public ChromelyResponse()
        {
            this.RequestId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyResponse"/> class.
        /// </summary>
        /// <param name="requestId">
        /// The request id.
        /// </param>
        public ChromelyResponse(string requestId)
        {
            this.RequestId = requestId;
        }

        /// <summary>
        /// Gets or sets the route path.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the ready state.
        /// </summary>
        public int ReadyState { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the status text.
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public object Data { get; set; }
    }
}
