// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteJavaScriptDemoController.cs" company="Chromely">
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
namespace Chromely.CefGlue.Winapi.netCoreDemo.Controllers
{
    using System;
    using Chromely.Core.RestfulService;
    using LitJson;

    using Xilium.CefGlue;

    /// <summary>
    /// The demo controller.
    /// </summary>
    [ControllerProperty(Name = "ExecuteJavaScriptDemoController", Route = "executejavascript")]
    public class ExecuteJavaScriptDemoController : ChromelyController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteJavaScriptDemoController"/> class.
        /// </summary>
        public ExecuteJavaScriptDemoController()
        {
            this.RegisterPostRequest("/executejavascript/execute", this.Execute);
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private  ChromelyResponse Execute(ChromelyRequest request)
        {
            var response = new ChromelyResponse(request.Id);
            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = DateTime.Now.ToLongDateString();

            try
            {
                ScriptInfo scriptInfo = new ScriptInfo(request.PostData);
                CefFrame frame = FrameHandler.GetFrame(scriptInfo.FrameName);
                if (frame == null)
                {
                    response.Data = $"Frame {scriptInfo.FrameName} does not exist.";
                    return response;
                }

                frame.ExecuteJavaScript(scriptInfo.Script, null, 0);
                response.Data = "Executed script :" + scriptInfo.Script;
                return response;
            }
            catch (Exception e)
            {
                response.ReadyState = (int)ReadyState.RequestReceived;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Error";
            }

            return response;
        }

        /// <summary>
        /// The script info.
        /// </summary>
        private class ScriptInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ScriptInfo"/> class.
            /// </summary>
            /// <param name="postData">
            /// The post data.
            /// </param>
            public ScriptInfo(object postData)
            {
                this.FrameName = string.Empty;
                this.Script = string.Empty;
                if (postData != null)
                {
                    JsonData jsonData = JsonMapper.ToObject(postData.ToString());
                    this.FrameName = jsonData.Keys.Contains("framename") ? jsonData["framename"].ToString() : string.Empty;
                    this.Script = jsonData.Keys.Contains("script") ? jsonData["script"].ToString() : string.Empty;
                }
            }

            /// <summary>
            /// Gets the frame name.
            /// </summary>
            public string FrameName { get; }

            /// <summary>
            /// Gets the script.
            /// </summary>
            public string Script { get; }
        }
    }
}
