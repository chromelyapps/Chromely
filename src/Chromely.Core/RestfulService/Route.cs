// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Route.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.RestfulService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The route.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        public Route(Method method, string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            Method = method;
            Path = path;
            Action = action;
        }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        public Method Method { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public Func<ChromelyRequest, ChromelyResponse> Action { get; set; }

        /// <summary>
        /// Gets or sets the action async.
        /// </summary>
        public Func<ChromelyRequest, Task<ChromelyResponse>> ActionAsync { get; set; }

        /// <summary>
        /// Invokes the registered action.
        /// </summary>
        /// <param name="requestId">
        /// The request identifier.
        /// </param>
        /// <param name="routePath">
        /// The route path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="postData">
        /// The post data.
        /// </param>
        /// <param name="rawJson">
        /// Raw json request data.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        public ChromelyResponse Invoke(string requestId, RoutePath routePath, IDictionary<string, object> parameters, object postData, string rawJson = null)
        {
            ChromelyRequest request = new ChromelyRequest(requestId, routePath, parameters, postData, rawJson);
            return Action.Invoke(request);
        }

        /// <summary>
        /// Invokes the registered action.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        public ChromelyResponse Invoke(ChromelyRequest request)
        {
            return Action.Invoke(request);
        }
    }
}
