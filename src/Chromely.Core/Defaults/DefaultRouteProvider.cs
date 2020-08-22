// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Logging;
using Chromely.Core.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chromely.Core.Defaults
{
    public class DefaultRouteProvider : IChromelyRouteProvider
    {
        public DefaultRouteProvider()
        {
            ActionRouteDictionary = new Dictionary<string, RequestActionRoute>();
            CommandRouteDictionary = new Dictionary<string, CommandActionRoute>();
        }

        public Dictionary<string, RequestActionRoute> ActionRouteDictionary { get; private set; }
        public Dictionary<string, CommandActionRoute> CommandRouteDictionary { get; private set; }

        public virtual void RegisterAllRoutes(List<ChromelyController> controllers)
        {
            if (controllers == null || !controllers.Any())
            {
                return;
            }

            try
            {
                var routeDictionary = new Dictionary<string, RequestActionRoute>();
                var commandDictionary = new Dictionary<string, CommandActionRoute>();

                foreach (var controller in controllers)
                {
                    var controllerFactory = new ChromelyControllerFactory();
                    var currentRouteDictionary = controller.ActionRouteDictionary;
                    var currentCommandDictionary = controller.CommandRouteDictionary;

                    // Merge with return route dictionary
                    if ((currentRouteDictionary != null) && currentRouteDictionary.Any())
                    {
                        foreach (var item in currentRouteDictionary)
                        {
                            if (!routeDictionary.ContainsKey(item.Key))
                            {
                                routeDictionary.Add(item.Key, item.Value);
                            }
                        }
                    }

                    // Merge with return command dictionary
                    if ((currentCommandDictionary != null) && currentCommandDictionary.Any())
                    {
                        foreach (var item in currentCommandDictionary)
                        {
                            if (!commandDictionary.ContainsKey(item.Key))
                            {
                                commandDictionary.Add(item.Key, item.Value);
                            }
                        }
                    }

                    // Add Http Attributes
                    var httpAttributeRoutes = controllerFactory.GetActionAttributeRoutes(controller);
                    if ((httpAttributeRoutes != null) && httpAttributeRoutes.Any())
                    {
                        foreach (var item in httpAttributeRoutes)
                        {
                            if (!routeDictionary.ContainsKey(item.Key))
                            {
                                routeDictionary.Add(item.Key, item.Value);
                            }
                        }
                    }

                    // Add Custom Attributes
                    var customAttributeRoutes = controllerFactory.GetCommandAttributeRoutes(controller);
                    if ((customAttributeRoutes != null) && customAttributeRoutes.Any())
                    {
                        foreach (var item in customAttributeRoutes)
                        {
                            if (!commandDictionary.ContainsKey(item.Key))
                            {
                                commandDictionary.Add(item.Key, item.Value);
                            }
                        }
                    }
                }

                RegisterActionRoutes(routeDictionary);
                RegisterCommandRoutes(commandDictionary);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, "RouteScanner:Scan");
            }
        }
        public virtual void RegisterActionRoute(string key, RequestActionRoute route)
        {
            EnsureContainersAreInitialized();
            ActionRouteDictionary.Add(key, route);
        }

        public virtual void RegisterCommandRoute(string key, CommandActionRoute command)
        {
            EnsureContainersAreInitialized();
            CommandRouteDictionary.Add(key, command);
        }

        public virtual void RegisterActionRoutes(Dictionary<string, RequestActionRoute> newRouteDictionary)
        {
            if (newRouteDictionary != null && newRouteDictionary.Any())
            {
                EnsureContainersAreInitialized();
                foreach (var item in newRouteDictionary)
                {
                    ActionRouteDictionary.Add(item.Key, item.Value);
                }
            }
        }

        public virtual void RegisterCommandRoutes(Dictionary<string, CommandActionRoute> newCommandDictionary)
        {
            if (newCommandDictionary != null && newCommandDictionary.Any())
            {
                EnsureContainersAreInitialized();
                foreach (var item in newCommandDictionary)
                {
                    CommandRouteDictionary.Add(item.Key, item.Value);
                }
            }
        }

        public virtual RequestActionRoute GetActionRoute(string routeUrl)
        {
            var key = RouteKey.CreateRequestKey(routeUrl);
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            EnsureContainersAreInitialized();
            if (ActionRouteDictionary.ContainsKey(key))
            {
                return ActionRouteDictionary[key];
            }

            return null;
        }

        public virtual CommandActionRoute GetCommandRoute(string commandUrl)
        {
            var key = RouteKey.CreateCommandKey(commandUrl);
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            EnsureContainersAreInitialized();
            if (CommandRouteDictionary.ContainsKey(key))
            {
                return CommandRouteDictionary[key];
            }

            return null;
        }

        public virtual bool IsActionRouteAsync(string routeUrl)
        {
            try
            {
                var route = GetActionRoute(routeUrl);
                return route == null ? false : route.IsAsync;
            }
            catch {}

            return false;
        }

        private void EnsureContainersAreInitialized()
        {
            if (ActionRouteDictionary == null)
            {
                ActionRouteDictionary = new Dictionary<string, RequestActionRoute>();
            }

            if (CommandRouteDictionary == null)
            {
                CommandRouteDictionary = new Dictionary<string, CommandActionRoute>();
            }
        }
    }
}