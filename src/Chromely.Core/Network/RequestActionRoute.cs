// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chromely.Core.Network
{
    public class RequestActionRoute
    {
        public RequestActionRoute(string path, Func<IChromelyRequest, IChromelyResponse> action, string description = null)
        {
            Action = action;
            Path = path;
            Description = description;
            IsAsync = false;
        }

        public RequestActionRoute(string path, Func<IChromelyRequest, Task<IChromelyResponse>> asyncAction, string description = null)
        {
            ActionAsync = asyncAction;
            Path = path;
            Description = description;
            IsAsync = true;
        }

        public bool IsAsync { get; private set; }
        public string Path { get; set; }
        public string Description { get; set; }

        public Func<IChromelyRequest, IChromelyResponse> Action { get; set; }
        public Func<IChromelyRequest, Task<IChromelyResponse>> ActionAsync { get; set; }
        public IChromelyResponse Invoke(string requestId, string routeUrl, IDictionary<string, string> parameters, object postData, string rawJson = null)
        {
            var request = new ChromelyRequest(requestId, routeUrl, parameters, postData, rawJson);
            return Action.Invoke(request);
        }
        public IChromelyResponse Invoke(IChromelyRequest request)
        {
            return Action.Invoke(request);
        }

        public Task<IChromelyResponse> InvokeAsync(string requestId, string routeUrl, IDictionary<string, string> parameters, object postData, string rawJson = null)
        {
            var request = new ChromelyRequest(requestId, routeUrl, parameters, postData, rawJson);
            return ActionAsync.Invoke(request);
        }

        public Task<IChromelyResponse> InvokeAsync(IChromelyRequest request)
        {
            return ActionAsync.Invoke(request);
        }
    }
}
