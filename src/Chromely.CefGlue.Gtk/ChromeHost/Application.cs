using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chromely.CefGlue.Gtk.ChromeHost
{
    internal static class Application
    {
        public static void Init()
        {
            string[] args = null;
            NativeMethods.InitWindow(args?.Length ?? 0, args);
        }

        public static void Run()
        {
            /* run the GTK+ main loop */
           NativeMethods.Run();
        }

        public static void Quit()
        {
            NativeMethods.Quit();
        }
    }
}
