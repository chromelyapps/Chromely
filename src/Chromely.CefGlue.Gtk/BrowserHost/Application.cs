// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppRunner.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
// See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.BrowserHost
{
    /// <summary>
    /// The application.
    /// </summary>
    internal static class Application
    {
        /// <summary>
        /// The init.
        /// </summary>
        public static void Init()
        {
            NativeMethods.InitWindow(0, null);
        }

        /// <summary>
        /// The run.
        /// </summary>
        public static void Run()
        {
            /* run the GTK+ main loop */
           NativeMethods.Run();
        }

        /// <summary>
        /// The quit.
        /// </summary>
        public static void Quit()
        {
            NativeMethods.Quit();
        }
    }
}
