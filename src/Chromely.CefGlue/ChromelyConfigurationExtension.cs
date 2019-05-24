// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyConfigurationExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using Chromely.Core;

namespace Chromely.CefGlue
{
    /// <summary>
    /// The chromely configuration extension.
    /// </summary>
    public static class ChromelyConfigurationExtension
    {
        /// <summary>
        /// The use default subprocess.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="useDefaultSubprocess">
        /// The use default subprocess.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public static ChromelyConfiguration UseDefaultSubprocess(this ChromelyConfiguration configuration, bool useDefaultSubprocess = true)
        {
            configuration.UseDefaultSubprocess = useDefaultSubprocess;

            if (useDefaultSubprocess)
            {
                // Disable security features
                configuration.WithCommandLineArg("default-encoding", "utf-8");
                configuration.WithCommandLineArg("allow-file-access-from-files");
                configuration.WithCommandLineArg("allow-universal-access-from-files");
                configuration.WithCommandLineArg("disable-web-security");
                configuration.WithCommandLineArg("ignore-certificate-errors");
            }

            return configuration;
        }

        /// <summary>
        /// The use default websocket handler.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="port">
        /// The port.
        /// </param>
        /// <param name="onLoadStartServer">
        /// The onLoadStartServer.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/>.
        /// </returns>
        public static ChromelyConfiguration UseDefaultWebsocketHandler(this ChromelyConfiguration configuration, string address, int port, bool onLoadStartServer)
        {
            configuration.WebsocketAddress = address;
            configuration.WebsocketPort = port;
            configuration.StartWebSocket = onLoadStartServer;
            return configuration;
        }
    }
}
