// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Text.Json;
using Chromely.Core.Infrastructure;
using Chromely.Core.Logging;
using Chromely.Core.Network;

namespace Chromely.CrossPlat.ChromelyControllers
{
    /// <summary>
    /// The demo controller.
    /// </summary>
    [ChromelyController(Name = "ExecuteJavaScriptDemoController")]
    public class ExecuteJavaScriptDemoController : ChromelyController
    {
        private readonly IChromelyConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteJavaScriptDemoController"/> class.
        /// </summary>
        public ExecuteJavaScriptDemoController(IChromelyConfiguration config)
        {
            _config = config;
        }

        [ChromelyRoute(Path = "/executejavascript/execute")]
        public IChromelyResponse Execute(string framename, string script)
        {
            var response = new ChromelyResponse()
            {
                ReadyState = (int)ReadyState.ResponseIsReady,
                Status = (int)System.Net.HttpStatusCode.OK,
                StatusText = "OK",
                Data = DateTime.Now.ToLongDateString()
            };

            try
            {
                var scriptExecutor = _config?.JavaScriptExecutor;
                if (scriptExecutor == null)
                {
                    response.Data = $"Frame {framename} does not exist.";
                    return response;
                }

                scriptExecutor.ExecuteScript(script);
                response.Data = "Executed script :" + script;
                return response;
            }
            catch (Exception e)
            {
                Logger.Instance.Log.LogError(e);
                response.Data = e.Message;
                response.ReadyState = (int)ReadyState.RequestReceived;
                response.Status = (int)System.Net.HttpStatusCode.BadRequest;
                response.StatusText = "Error";
            }

            return response;
        }
    }
}
