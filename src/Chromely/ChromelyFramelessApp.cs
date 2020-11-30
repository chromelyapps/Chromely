// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.NativeHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.ComponentModel;

namespace Chromely
{
    /// <summary>
    /// Simplest Chromely Frameless app implementation.
    /// Be sure to call base implementations on derived implementations.
    /// </summary>
    public class ChromelyFramelessApp : ChromelyAppBase
    {
        public sealed override void ConfigureCoreServices(IServiceCollection services)
        {
            base.ConfigureCoreServices(services);

            var platform = ChromelyRuntime.Platform;
            if (platform != ChromelyPlatform.Windows)
            {
                throw new InvalidOperationException("ChromelyFramelessApp can only be used on Windows.");
            }

            services.TryAddSingleton<IChromelyNativeHost, ChromelyWinFramelessHost>();
        }
    }
}
