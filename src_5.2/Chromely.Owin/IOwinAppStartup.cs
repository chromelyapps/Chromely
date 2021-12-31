// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Owin;

/// <summary>
/// Represents the Owin startup.
/// </summary>
public interface IOwinAppStartup
{
    /// <summary>
    /// Gets or sets the environent.
    /// </summary>
    string? Environment { get; set; }
    /// <summary>
    /// Gets or sets the error handling path.
    /// </summary>
    string? ErrorHandlingPath { get; set; }
    /// <summary>
    /// Gets or sets Owin/Asp.NET Core configuration.
    /// </summary>
    IConfiguration? Configuration { get; set; }

    /// <summary>
    /// Configure the configuration builder.
    /// </summary>
    /// <param name="configBuilder">The <see cref="IConfigurationBuilder"/> instance.</param>
    void Configure(IConfigurationBuilder configBuilder);
    /// <summary>
    /// The primary way to add services. All default handlers should be overriden here using custom handlers.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    void ConfigureServices(IServiceCollection services);
    /// <summary>
    /// Configure other primary Owin services.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <param name="env">The <see cref="IWebHostEnvironment"/> instance.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> instance.</param>
    void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory);
    /// <summary>
    /// Configure other primary Owin services using the host builder.
    /// </summary>
    /// <param name="builder">The <see cref="IWebHostBuilder"/> instance.</param>
    void Configure(IWebHostBuilder builder);
    /// <summary>
    /// Configure other primary Owin services using the host.
    /// </summary>
    /// <param name="host">The <see cref="IWebHost"/> instance.</param>
    void Configure(IWebHost host);
}