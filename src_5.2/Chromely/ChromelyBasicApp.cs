// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

/// <summary>
/// Simplest Chromely app implementation.
/// Be sure to call base implementations on derived implementations.
/// </summary>
public class ChromelyBasicApp : ChromelyAppBase
{
    public sealed override void ConfigureCoreServices(IServiceCollection services)
    {
        base.ConfigureCoreServices(services);
    }

    public sealed override void ConfigureDefaultHandlers(IServiceCollection services)
    {
        base.ConfigureDefaultHandlers(services);
    }
}