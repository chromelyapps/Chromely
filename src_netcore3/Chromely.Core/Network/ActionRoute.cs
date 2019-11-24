// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Route.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chromely.Core.Network
{
    public class ActionRoute
    {
        public ActionRoute(Method method, string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            Method = method;
            Path = path;
            Action = action;
        }

        public ActionRoute(Method method, string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            Method = method;
            Path = path;
            ActionAsync = action;
            IsAsync = true;
        }

        public bool IsAsync { get; private set; }
        public Method Method { get; set; }
        public string Path { get; set; }
        public Func<ChromelyRequest, ChromelyResponse> Action { get; set; }
        public Func<ChromelyRequest, Task<ChromelyResponse>> ActionAsync { get; set; }
        public ChromelyResponse Invoke(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string rawJson = null)
        {
            ChromelyRequest request = new ChromelyRequest(requestId, routePath, parameters, postData, rawJson);
            return Action.Invoke(request);
        }
        public ChromelyResponse Invoke(ChromelyRequest request)
        {
            return Action.Invoke(request);
        }

        public Task<ChromelyResponse> InvokeAsync(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string rawJson = null)
        {
            ChromelyRequest request = new ChromelyRequest(requestId, routePath, parameters, postData, rawJson);
            return ActionAsync.Invoke(request);
        }

        public Task<ChromelyResponse> InvokeAsync(ChromelyRequest request)
        {
            return ActionAsync.Invoke(request);
        }

    }
}
