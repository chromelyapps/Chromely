// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostHelpers.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.Core.Infrastructure;
using WinApi.Windows;
// ReSharper disable UnusedMember.Global

namespace Chromely.CefGlue.Winapi
{
    /// <summary>
    /// The host helpers.
    /// </summary>
    public static class HostHelpers
    {
        /// <summary>
        /// Initializes static members of the <see cref="HostHelpers"/> class.
        /// </summary>
        static HostHelpers()
        {
            DefaultUnhandledExceptionHandler =
                (sender, eventArgs) =>
                {
                    Log.Error(eventArgs.ExceptionObject as Exception);
                };

            DefaultWindowExceptionHandler = ex =>
            {
                Log.Error(ex.InnerException);
                ex.SetHandled();
            };
        }

        /// <summary>
        /// Gets the default window exception handler.
        /// </summary>
        public static WindowExceptionHandler DefaultWindowExceptionHandler { get; }

        /// <summary>
        /// Gets the default unhandled exception handler.
        /// </summary>
        public static UnhandledExceptionEventHandler DefaultUnhandledExceptionHandler { get; }

        /// <summary>
        /// The setup default exception handlers.
        /// </summary>
        public static void SetupDefaultExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += DefaultUnhandledExceptionHandler;
            WindowCore.UnhandledException += DefaultWindowExceptionHandler;
        }
    }
}