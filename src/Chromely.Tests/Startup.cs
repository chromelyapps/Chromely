// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Tests;

internal class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IChromelyDataTransferOptions, DataTransferOptions>();
        services.AddTransient<IChromelyModelBinder, DefaultModelBinder>();
        services.AddTransient<IChromelyRouteProvider, DefaultRouteProvider>();
        services.RegisterChromelyControllerAssembly(typeof(Startup).Assembly, ServiceLifetime.Singleton);
        services.AddDbContextFactory<TodoContext>();
    }
}