// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Owin;

/// <summary>
/// The actual Owin startup class. 
/// </summary>
internal sealed class OwinStartup
{
    private readonly IOwinAppStartup _owinApp;

    /// <summary>
    /// Initializes a new instance of <see cref="OwinStartup"/>
    /// </summary>
    /// <param name="owinApp">Instance of <see cref="IOwinAppStartup"/>.</param>
    public OwinStartup(IOwinAppStartup owinApp)
    {
        _owinApp = owinApp;
    }


    /// <summary>
    /// Configures the application.
    /// </summary>
    /// <param name="app">An <see cref="IApplicationBuilder"/> instance for the app to configure.</param>
    /// <param name="env">The application <see cref="IWebHostEnvironment"/>.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        _owinApp.Configure(app, env, loggerFactory);
    }
}