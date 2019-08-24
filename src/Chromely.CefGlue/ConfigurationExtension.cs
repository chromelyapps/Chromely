// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyConfigurationExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core;
using Chromely.Core.Infrastructure;

namespace Chromely.CefGlue
{
    /// <summary>
    /// The chromely configuration extension.
    /// </summary>
    public static class ConfigurationExtension
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
        /// <param name="configuration">
        /// The configuration.
        /// </param>
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

        /// <summary>
        /// Registers message router handler.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="messageRouterHandler">
        /// The chromely message router handler.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public static ChromelyConfiguration RegisterMessageRouterHandler(this ChromelyConfiguration configuration, object messageRouterHandler)
        {
            return configuration.RegisterMessageRouterHandler(new ChromelyMessageRouter(messageRouterHandler));
        }

        /// <summary>
        /// Registers message router handler.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="messageRouterHandler">
        /// The chromely message router.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyConfiguration"/> object.
        /// </returns>
        public static ChromelyConfiguration RegisterMessageRouterHandler(this ChromelyConfiguration configuration, ChromelyMessageRouter messageRouterHandler)
        {
            if (messageRouterHandler != null)
            {
                IoC.RegisterInstance(typeof(ChromelyMessageRouter), messageRouterHandler.Key, messageRouterHandler);
            }

            return configuration;
        }


        /// <summary>
        /// The register websocket handler.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="socketHandler">
        /// The socket handler.
        /// </param>
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
        public static ChromelyConfiguration RegisterWebsocketHandler(this ChromelyConfiguration configuration, IChromelyWebsocketHandler socketHandler, string address, int port, bool onLoadStartServer)
        {
            if (socketHandler == null)
            {
                return configuration;
            }

            // Remove handler if exists - only one handler is allowed.
            var isHandlerRegistered = IoC.IsRegistered<IChromelyWebsocketHandler>(typeof(IChromelyWebsocketHandler).FullName);
            if (isHandlerRegistered)
            {
                IoC.UnregisterHandler<IChromelyWebsocketHandler>(typeof(IChromelyWebsocketHandler).FullName);
            }

            IoC.RegisterInstance(typeof(IChromelyWebsocketHandler), typeof(IChromelyWebsocketHandler).FullName, socketHandler);

            configuration.WebsocketAddress = address;
            configuration.WebsocketPort = port;
            configuration.StartWebSocket = onLoadStartServer;
            return configuration;
        }
    }
}
