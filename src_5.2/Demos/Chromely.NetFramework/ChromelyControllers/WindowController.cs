// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Network;
using Chromely.Core.Logging;
using Chromely.Core.Infrastructure;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Chromely.NetFramework.ChromelyControllers
{
    [ChromelyController(Name = "WindowController")]
    public class WindowController : ChromelyController
    {
        private const string COMMANDMAXRESPONSE = "toggleRestoreMaxButton(1);";
        private const string COMMANDRESTORERESPONSE = "toggleRestoreMaxButton(0);";

        private readonly IChromelyConfiguration _config;
        private readonly IChromelyWindow _window;

        public WindowController(IChromelyConfiguration config, IChromelyWindow window)
        {
            _config = config;
            _window = window;
        }

        [ChromelyRoute(Path = "/window/minimize")]
        public void Minimize()
        {
            _window?.Minimize();
        }

        [ChromelyRoute(Path = "/window/maximize")]
        public void Maximize()
        {
            _window?.Maximize();
            RunResponseScript(COMMANDMAXRESPONSE);
        }

        [ChromelyRoute(Path = "/window/restore")]
        public void Restore()
        {
            _window?.Restore();
            RunResponseScript(COMMANDRESTORERESPONSE);
        }

        [ChromelyRoute(Path = "/window/close")]
        public void Close(IDictionary<string, string> queryParameters)
        {
            _window?.Close();
        }

        private void RunResponseScript(string script)
        {
            try
            {
                Task.Run(() =>
                {
                    var scriptExecutor = _config?.JavaScriptExecutor;
                    if (scriptExecutor != null)
                    {
                        scriptExecutor.ExecuteScript(script);
                    }

                });
            }
            catch (Exception e)
            {
                Logger.Instance.Log.LogError(e);
            }

        }
    }
}
