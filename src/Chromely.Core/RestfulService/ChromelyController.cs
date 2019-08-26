// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyController.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Chromely.Core.Infrastructure;

namespace Chromely.Core.RestfulService
{


    /// <summary>
    /// The chromely controller.
    /// </summary>
    public abstract class ChromelyController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyController"/> class.
        /// </summary>
        protected ChromelyController()
        {
            RouteDictionary = new Dictionary<string, Route>();
            CommandDictionary = new Dictionary<string, Command>();
        }

        /// <summary>
        /// Gets the route dictionary.
        /// </summary>
        public Dictionary<string, Route> RouteDictionary { get; }

        /// <summary>
        /// Gets the command dictionary.
        /// </summary>
        public Dictionary<string, Command> CommandDictionary { get; }

        /// <summary>
        /// Gets the route name.
        /// </summary>
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

        /// <summary>
        /// Gets the route path.
        /// </summary>
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

        /// <summary>
        /// The register get request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterGetRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.GET, path, new Route(Method.GET, path, action));
        }

        /// <summary>
        /// The register get request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterGetRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.GET, path, new Route(Method.GET, path, action));
        }

        /// <summary>
        /// The register post request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterPostRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.POST, path, new Route(Method.POST, path, action));
        }

        /// <summary>
        /// The register post request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterPostRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.POST, path, new Route(Method.POST, path, action));
        }

        /// <summary>
        /// The register put request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterPutRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.PUT, path, new Route(Method.PUT, path, action));
        }

        /// <summary>
        /// The register put request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterPutRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.PUT, path, new Route(Method.PUT, path, action));
        }

        /// <summary>
        /// The register delete request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterDeleteRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.DELETE, path, new Route(Method.DELETE, path, action));
        }

        /// <summary>
        /// The register delete request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterDeleteRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.DELETE, path, new Route(Method.DELETE, path, action));
        }

        /// <summary>
        /// The register head request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterHeadRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.HEAD, path, new Route(Method.HEAD, path, action));
        }

        /// <summary>
        /// The register head request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterHeadRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.HEAD, path, new Route(Method.HEAD, path, action));
        }

        /// <summary>
        /// The register options request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterOptionsRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.OPTIONS, path, new Route(Method.OPTIONS, path, action));
        }

        /// <summary>
        /// The register options request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterOptionsRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.OPTIONS, path, new Route(Method.OPTIONS, path, action));
        }

        /// <summary>
        /// The register patch request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterPatchRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.PATCH, path, new Route(Method.PATCH, path, action));
        }

        /// <summary>
        /// The register patch request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterPatchRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.PATCH, path, new Route(Method.PATCH, path, action));
        }

        /// <summary>
        /// The register merge request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterMergeRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(Method.MERGE, path, new Route(Method.MERGE, path, action));
        }

        /// <summary>
        /// The register merge request.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterMergeRequestAsync(string path, Func<ChromelyRequest, Task<ChromelyResponse>> action)
        {
            AddRoute(Method.MERGE, path, new Route(Method.MERGE, path, action));
        }

        /// <summary>
        /// The register command.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void RegisterCommand(string path, Action<IDictionary<string, string[]>> action)
        {
            if (string.IsNullOrWhiteSpace(path) || action == null)
            {
                return;
            }
            
            var command = new Command(path, action);
            CommandDictionary[command.Key] = command;
        }

        /// <summary>
        /// The add route.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="route">
        /// The route.
        /// </param>
        protected void AddRoute(Method method, string path, Route route)
        {
            var routhPath = new RoutePath(method, path);
            RouteDictionary[routhPath.Key] = route;
        }

        /// <summary>
        /// The add route.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="route">
        /// The route.
        /// </param>
        protected void AddRoute(string method, string path, Route route)
        {
            var routhPath = new RoutePath(method, path);
            RouteDictionary[routhPath.Key] = route;
        }

        /// <summary>
        /// The get attribute info.
        /// </summary>
        /// <returns>
        /// The <see cref="Tuple"/>.
        /// </returns>
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
                Log.Error(exception);
            }

            return null;
        }
    }
}
