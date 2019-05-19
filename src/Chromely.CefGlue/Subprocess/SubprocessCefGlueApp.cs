// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubprocessCefGlueApp.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Chromely.CefGlue.Browser.Handlers;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Subprocess
{
    /// <summary>
    /// The Subprocess CefGlue app.
    /// </summary>
    internal class SubprocessCefGlueApp : CefApp
    {
        /// <summary>
        /// The render process handler.
        /// </summary>
        private readonly CefRenderProcessHandler mRenderProcessHandler = new CefGlueRenderProcessHandler();

        /// <summary>
        /// 
        /// </summary>
        private readonly SubprocessParams mSubprocessParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubprocessCefGlueApp"/> class.
        /// </summary>
        /// <param name="subprocessParams">
        /// Subprocess params
        /// </param>
        public SubprocessCefGlueApp(SubprocessParams subprocessParams)
        {
            mSubprocessParams = subprocessParams;
        }

        /// <summary>
        /// The on register custom schemes.
        /// </summary>
        /// <param name="registrar">
        /// The registrar.
        /// </param>
        protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
        {
            if (mSubprocessParams == null && mSubprocessParams.CustomSchemes != null && mSubprocessParams.CustomSchemes.Any())
            {
                foreach (var item in mSubprocessParams.CustomSchemes)
                {
                    registrar.AddCustomScheme(item, true, false, false, false, true, false);
                }
            }
        }

        /// <summary>
        /// The on before command line processing.
        /// </summary>
        /// <param name="processType">
        /// The process type.
        /// </param>
        /// <param name="commandLine">
        /// The command line.
        /// </param>
        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            if (mSubprocessParams == null && mSubprocessParams.CommandLineArgs != null && mSubprocessParams.CommandLineArgs.Any())
            {
                foreach (var item in mSubprocessParams.CommandLineArgs)
                {
                    if (item.Item3 && !string.IsNullOrWhiteSpace(item.Item1))
                    {
                        commandLine.AppendSwitch(item.Item1, item.Item2);
                    }
                    else if (!string.IsNullOrWhiteSpace(item.Item2))
                    {
                        commandLine.AppendSwitch(item.Item2);
                    }
                }
            }

            // Currently on linux platform location of locales and pack files are determined
            // incorrectly (relative to main module instead of libcef.so module).
            // Once issue http://code.google.com/p/chromiumembedded/issues/detail?id=668 will be resolved
            // this code can be removed.
            if (CefRuntime.Platform == CefRuntimePlatform.Linux)
            {
                var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

                commandLine.AppendSwitch("resources-dir-path", path);
                commandLine.AppendSwitch("locales-dir-path", Path.Combine(path, "locales"));
                commandLine.AppendSwitch("disable-extensions", "1");
                commandLine.AppendSwitch("disable-gpu", "1");
                commandLine.AppendSwitch("disable-gpu-compositing", "1");
                commandLine.AppendSwitch("disable-smooth-scrolling", "1");
                commandLine.AppendSwitch("no-sandbox", "1");
                commandLine.AppendSwitch("no-zygote", "1");
            }
        }

        /// <summary>
        /// The get render process handler.
        /// </summary>
        /// <returns>
        /// The <see cref="CefRenderProcessHandler"/>.
        /// </returns>
        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return mRenderProcessHandler;
        }
    }
}
