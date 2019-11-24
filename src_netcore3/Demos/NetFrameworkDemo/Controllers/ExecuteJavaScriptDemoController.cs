// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteJavaScriptDemoController.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text.Json;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;

namespace NetFrameworkDemo.Controllers
{
    /// <summary>
    /// The demo controller.
    /// </summary>
    [ControllerProperty(Name = "ExecuteJavaScriptDemoController", Route = "executejavascript")]
    public class ExecuteJavaScriptDemoController : ChromelyController
    {
        private readonly IChromelyConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteJavaScriptDemoController"/> class.
        /// </summary>
        public ExecuteJavaScriptDemoController(IChromelyConfiguration config)
        {
            _config = config;
            RegisterPostRequest("/executejavascript/execute", Execute);
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
        private ChromelyResponse Execute(ChromelyRequest request)
        {
            var response = new ChromelyResponse(request.Id)
            {
                ReadyState = (int)ReadyState.ResponseIsReady,
                Status = (int)System.Net.HttpStatusCode.OK,
                StatusText = "OK",
                Data = DateTime.Now.ToLongDateString()
            };

            try
            {
                var scriptInfo = new ScriptInfo(request.PostData);
                var scriptExecutor = _config?.JavaScriptExecutor;
                if (scriptExecutor == null)
                {
                    response.Data = $"Frame {scriptInfo.FrameName} does not exist.";
                    return response;
                }

                scriptExecutor.ExecuteScript(scriptInfo.Script);
                response.Data = "Executed script :" + scriptInfo.Script;
                return response;
            }
            catch (Exception e)
            {
                Logger.Instance.Log.Error(e);
                response.Data = e.Message;
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
                FrameName = string.Empty;
                Script = string.Empty;
                if (postData != null)
                {
                    var options = new JsonSerializerOptions();
                    options.ReadCommentHandling = JsonCommentHandling.Skip;
                    options.AllowTrailingCommas = true;
                    var requestData = JsonSerializer.Deserialize<scriptInfo>(postData.ToString(), options);

                    FrameName = requestData.framename ?? string.Empty;
                    Script = requestData.script ?? string.Empty;
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

        private class scriptInfo
        {
            public string framename { get; set; }
            public string script { get; set; }
        }
    }
}
