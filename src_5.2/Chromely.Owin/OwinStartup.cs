// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Owin;

/// <summary>
/// The actual Owin startup class. 
/// </summary>
internal sealed class OwinStartup
{
    private readonly IOwinAppStartup _owinApp;

    public OwinStartup(IOwinAppStartup owinApp)
    {
        _owinApp = owinApp;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        _owinApp.Configure(app, env, loggerFactory);
    }
}