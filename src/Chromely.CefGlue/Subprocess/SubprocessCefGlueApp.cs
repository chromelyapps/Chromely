// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubprocessCefGlueApp.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

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
        private readonly CefRenderProcessHandler _renderProcessHandler = new CefGlueRenderProcessHandler();

        /// <summary>
        /// 
        /// </summary>
        private readonly SubprocessParams _subprocessParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubprocessCefGlueApp"/> class.
        /// </summary>
        /// <param name="subprocessParams">
        /// Subprocess params
        /// </param>
        public SubprocessCefGlueApp(SubprocessParams subprocessParams)
        {
            _subprocessParams = subprocessParams;
        }

        /// <summary>
        /// The on register custom schemes.
        /// </summary>
        /// <param name="registrar">
        /// The registrar.
        /// </param>
        protected override void OnRegisterCustomSchemes(CefSchemeRegistrar registrar)
        {
            if (_subprocessParams != null && _subprocessParams.CustomSchemes != null && _subprocessParams.CustomSchemes.Any())
            {
                foreach (var item in _subprocessParams.CustomSchemes)
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
            if (_subprocessParams != null && _subprocessParams.CommandLineArgs != null && _subprocessParams.CommandLineArgs.Any())
            {
                foreach (var item in _subprocessParams.CommandLineArgs)
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
            return _renderProcessHandler;
        }
    }
}
