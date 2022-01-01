// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely;
using Chromely.Core;
using Chromely.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace Chromely.NetFramework
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var config = DefaultConfiguration.CreateForRuntimePlatform();
            //config.StartUrl = "https://google.com";
            config.StartUrl = "local://app/index.html";

            // Frameless/draggable
            //config.StartUrl = "local://app/index_frameless.html";

            // config.StartUrl = "local://app/index_draggable.html";
            // config.WindowOptions.FramelessOption.UseWebkitAppRegions = true;

            AppBuilder
            .Create(args)
            .UseConfig<DefaultConfiguration>(config)
            .UseWindow<WindowSample>()
            .UseApp<AppSample>()
            .Build()
            .Run();
        }
    }
}