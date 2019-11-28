using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;

namespace Chromely.Core.Network
{
    public abstract class ChromelyController
    {
        protected ChromelyController()
        {
            ActionRouteDictionary = new Dictionary<string, ActionRoute>();
            CommandRouteDictionary = new Dictionary<string, CommandRoute>();
        }

        public Dictionary<string, ActionRoute> ActionRouteDictionary { get; }
        public Dictionary<string, CommandRoute> CommandRouteDictionary { get; }
        public string RouteName
        {
            get
            {
                var attributeInfo = GetAttributeInfo();
                if (attributeInfo != null)
                {
                    return attributeInfo.Item1;
                }

                return string.Empty;
            }
        }

        public string Path
        {
            get
            {
                var attributeInfo = GetAttributeInfo();
                if (attributeInfo != null)
                {
                    return attributeInfo.Item2;
                }

                return string.Empty;
            }
        }

        protected void RegisterGetRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.GET, path, new ActionRoute(Method.GET, path, action));
        }

        protected void RegisterGetRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.GET, path, new ActionRoute(Method.GET, path, action));
        }

        protected void RegisterPostRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.POST, path, new ActionRoute(Method.POST, path, action));
        }

        protected void RegisterPostRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.POST, path, new ActionRoute(Method.POST, path, action));
        }

        protected void RegisterPutRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.PUT, path, new ActionRoute(Method.PUT, path, action));
        }

        protected void RegisterPutRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.PUT, path, new ActionRoute(Method.PUT, path, action));
        }

        protected void RegisterDeleteRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.DELETE, path, new ActionRoute(Method.DELETE, path, action));
        }

        protected void RegisterDeleteRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.DELETE, path, new ActionRoute(Method.DELETE, path, action));
        }

        protected void RegisterHeadRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.HEAD, path, new ActionRoute(Method.HEAD, path, action));
        }

        protected void RegisterHeadRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.HEAD, path, new ActionRoute(Method.HEAD, path, action));
        }

        protected void RegisterOptionsRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.OPTIONS, path, new ActionRoute(Method.OPTIONS, path, action));
        }

        protected void RegisterOptionsRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.OPTIONS, path, new ActionRoute(Method.OPTIONS, path, action));
        }

        protected void RegisterPatchRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.PATCH, path, new ActionRoute(Method.PATCH, path, action));
        }

        protected void RegisterPatchRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.PATCH, path, new ActionRoute(Method.PATCH, path, action));
        }

        protected void RegisterMergeRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.MERGE, path, new ActionRoute(Method.MERGE, path, action));
        }

        protected void RegisterMergeRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.MERGE, path, new ActionRoute(Method.MERGE, path, action));
        }

        protected void RegisterCommand(string path, Action<IDictionary<string, string>> action)
        {
            if (string.IsNullOrWhiteSpace(path) || action == null)
            {
                return;
            }
            
            var command = new CommandRoute(path, action);
            CommandRouteDictionary[command.Key] = command;
        }

        protected void AddRoute(Method method, string path, ActionRoute route)
        {
            var routhPath = new RoutePath(method, path);
            ActionRouteDictionary[routhPath.Key] = route;
        }

        protected void AddRoute(string method, string path, ActionRoute route)
        {
            var routhPath = new RoutePath(method, path);
            ActionRouteDictionary[routhPath.Key] = route;
        }

        private Tuple<string, string> GetAttributeInfo()
        {
            try
            {
                var attribute = GetType().GetCustomAttribute<ControllerPropertyAttribute>(true);
                if (attribute != null)
                {
                    return new Tuple<string, string>(attribute.Name, attribute.Route);
                }

                return null;
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.Error(exception);
            }

            return null;
        }
    }
}
