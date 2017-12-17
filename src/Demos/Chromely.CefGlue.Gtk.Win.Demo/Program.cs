using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chromely.CefGlue.Gtk.Win.Demo
{
    using Chromely.CefGlue.Gtk.ChromeHost;

    class Program
    {
        [STAThread]
        private static int Main(string[] args)
        {
            using (var app = new CefGlueBrowserHost())
            {
                return app.Run(args);
            }
        }
    }
}
