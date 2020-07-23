// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Chromely.Core.Network
{
    public abstract class ChromelyController
    {
        protected ChromelyController()
        {
            ActionRouteDictionary = new Dictionary<string, RequestActionRoute>();
            CommandRouteDictionary = new Dictionary<string, CommandActionRoute>();
        }

        private string _name;
        private string _description;

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                {
                    SetAttributeInfo();
                }

                return _name;
            }
        }

        public string Description
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_description))
                {
                    SetAttributeInfo();
                }

                return _description;
            }
        }

        public Dictionary<string, RequestActionRoute> ActionRouteDictionary { get; }
        public Dictionary<string, CommandActionRoute> CommandRouteDictionary { get; }

        protected void RegisterRequest(string path, Func<IChromelyRequest, IChromelyResponse> action, string description = null)
        {
            AddRoute(path, new RequestActionRoute(path, action, description));
        }

        protected void RegisterRequestAsync(string path, Func<IChromelyRequest, Task<IChromelyResponse>> action, string description = null)
        {
            AddRoute(path, new RequestActionRoute(path, action, description));
        }

        protected void RegisterCommand(string path, Action<IDictionary<string, string>> action, string description = null)
        {
            if (string.IsNullOrWhiteSpace(path) || action == null)
            {
                return;
            }

            var commandKey = RouteKey.CreateCommandKey(path);
            var command = new CommandActionRoute(path, action, description);
            CommandRouteDictionary[commandKey] = command;
        }

        private void AddRoute(string path, RequestActionRoute route)
        {
            var actionKey = RouteKey.CreateRequestKey(path);
            ActionRouteDictionary[actionKey] = route;
        }

        private void SetAttributeInfo()
        {
            try
            {
                var attribute = GetType().GetCustomAttribute<ControllerPropertyAttribute>(true);
                if (attribute != null)
                {
                    _name = attribute.Name;
                    _description = attribute.Description;
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError("ChromelyController:SetAttributeInfo", exception);
            }
        }
    }
}
