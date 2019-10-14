// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global
namespace Chromely.Core.Host
{
    using System;

    using Helpers;
    using RestfulService;

    /// <summary>
    /// The IChromelyWindow interface.
    /// </summary>
    public interface IChromelyWindow : IChromelyServiceProvider, IDisposable
    {
        /// <summary>
        /// Gets the host config.
        /// </summary>
        ChromelyConfiguration HostConfig { get; }

        /// <summary>
        /// Gets the window handle.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        object Browser { get; }

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int Run(string[] args);

        /// <summary>
        /// The register event handler.
        /// The event handler must be registered before calling "Run".
        /// Alternatively this can be done before window is created during ChromelyConfiguration instantiation.
        /// Only one type of event handler can be registered. The first one is valid, consequent registrations will be ignored.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <typeparam name="T">
        /// This is the event argument class - e,g - LoadErrorEventArgs, FrameLoadStartEventArgs. 
        /// </typeparam>
        void RegisterEventHandler<T>(CefEventKey key, EventHandler<T> handler);

        /// <summary>
        /// The register event handler.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <typeparam name="T">
        /// This is the event argument class - e,g - LoadErrorEventArgs, FrameLoadStartEventArgs. 
        /// </typeparam>
        void RegisterEventHandler<T>(CefEventKey key, ChromelyEventHandler<T> handler);

        /// <summary>
        /// The register custom handler. 
        /// The custom handler must be registered before calling "Run".
        /// Alternatively this can be done before window is created during ChromelyConfiguration instantiation.
        /// Only one type of custom handler can be registered. The first one is valid, consequent registrations will be ignored.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="implementation">
        /// The implementation.
        /// </param>
        void RegisterCustomHandler(CefHandlerKey key, Type implementation);

        /// <summary>
        /// The close.
        /// </summary>
        void Close();

        /// <summary>
        /// The shutdown.
        /// </summary>
        void Exit();
    }
}