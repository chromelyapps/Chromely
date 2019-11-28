// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyRequest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Chromely.Core.Network
{
    public class ChromelyRequest
    {
        public ChromelyRequest(RoutePath routePath, IDictionary<string, string> parameters, object postData)
        {
            RoutePath = routePath;
            Parameters = parameters;
            PostData = postData;
        }

        public ChromelyRequest(string id, RoutePath routePath, IDictionary<string, string> parameters, object postData)
        {
            Id = id;
            RoutePath = routePath;
            Parameters = parameters;
            PostData = postData;
        }

        public ChromelyRequest(string id, RoutePath routePath, IDictionary<string, string> parameters, object postData, string rawJson)
        {
            Id = id;
            RoutePath = routePath;
            Parameters = parameters;
            PostData = postData;
            RawJson = rawJson;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the route path.
        /// </summary>
        public RoutePath RoutePath { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the post data.
        /// </summary>
        public object PostData { get; set; }

        /// <summary>
        /// Gets or sets the raw json.
        /// Only used for CefGlue Generic Message Routing requests.
        /// </summary>
        public string RawJson { get; set; }
    }
}
