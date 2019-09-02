// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestTaskRunner.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Chromely.Core.Infrastructure;

// ReSharper disable StyleCop.SA1210
namespace Chromely.Core.RestfulService
{
    /// <summary>
    /// The command task runner.
    /// </summary>
    public static class CommandTaskRunner
    {
        /// <summary>
        /// The run command async.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static void RunAsync(string url)
        {
            Task.Run(() =>
            {
                try
                {
                    var commandPath = GetPathFromUrl(url);
                    var command = ServiceRouteProvider.GetCommand(commandPath);

                    if (command == null)
                    {
                        Log.Error($"Command for path = {commandPath} is null or invalid.");
                        return;
                    }

                    var queryParameters = GetQueryParameters(url);
                    command.Invoke(queryParameters);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            });
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
        private static IDictionary<string, string[]> GetQueryParameters(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new Dictionary<string, string[]>();
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
                return new Dictionary<string, string[]>();
            }

            return nameValueCollection.AllKeys.ToDictionary(x => x, x => nameValueCollection.GetValues(x));
        }
    }
}