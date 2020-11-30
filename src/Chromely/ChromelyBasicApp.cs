// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.NativeHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chromely
{
    /// <summary>
    /// Simplest Chromely app implementation.
    /// Be sure to call base implementations on derived implementations.
    /// </summary>
    public class ChromelyBasicApp: ChromelyAppBase
    {
        public sealed override void ConfigureCoreServices(IServiceCollection services)
        {
            base.ConfigureCoreServices(services);

            var platform = ChromelyRuntime.Platform;

            switch (platform)
            {
                case ChromelyPlatform.MacOSX:
                    services.TryAddSingleton<IChromelyNativeHost, ChromelyMacHost>();
                    break;

                case ChromelyPlatform.Linux:
                    services.TryAddSingleton<IChromelyNativeHost, ChromelyLinuxHost>();
                    break;

                case ChromelyPlatform.Windows:
                    services.TryAddSingleton<IChromelyNativeHost, ChromelyWinHost>();
                    break;

                default:
                    services.TryAddSingleton<IChromelyNativeHost, ChromelyWinHost>();
                    break;
            }
        }
    }
}
