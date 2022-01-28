// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Chromely.Core.Logging;
using Chromely.Core.Network;
using Microsoft.Extensions.Logging;

namespace Chromely.Core.Defaults
{
    public class DefaultCommandTaskRunner : IChromelyCommandTaskRunner
    {
        protected readonly IChromelyRouteProvider _routeProvider;

        public DefaultCommandTaskRunner(IChromelyRouteProvider routeProvider)
        {
            _routeProvider = routeProvider;
        }

        public void RunAsync(string url)
        {
            Task.Run(() =>
            {
                Run(url);
            });
        }

        public void Run(string url)
        {
            try
            {
                var commandPath = GetPathFromUrl(url);
                var command = _routeProvider.GetCommandRoute(commandPath);

                if (command == null)
                {
                    Logger.Instance.Log.LogError($"Command for path = {commandPath} is null or invalid.");
                    return;
                }

                var queryParameters = GetQueryParameters(url);
                command.Invoke(queryParameters);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, "DefaultCommandTaskRunner:Run");
            }
        }

        private static string GetPathFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                return uri.AbsolutePath;
            }
            catch { }

            return url;
        }

        /// <summary>
        /// The get query parameters.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The name value collection.
        /// </returns>
        private static IDictionary<string, string> GetQueryParameters(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new Dictionary<string, string>();
            }

            var nameValueCollection = new NameValueCollection();

            string querystring = string.Empty;
            int index = url.IndexOf('?');
            if (index > 0)
            {
                querystring = url.Substring(url.IndexOf('?'));
                nameValueCollection = HttpUtility.ParseQueryString(querystring);
            }

            if (string.IsNullOrEmpty(querystring))
            {
                return new Dictionary<string, string>();
            }

            return nameValueCollection.AllKeys.ToDictionary(x => x, x => nameValueCollection[x]);
        }
    }
}