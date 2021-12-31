
internal class OwinAppSample : ChromelyOwinApp
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        // Add services to the container.
        services.AddRazorPages();

        services.AddLogging(configure => configure.AddConsole());
        services.AddLogging(configure => configure.AddFilter("Logs/chromely_serilog-{Date}.txt", LogLevel.Information));

        services.AddDbContextFactory<MovieContext>();

        RegisterChromelyControllerAssembly(services, typeof(OwinAppSample).Assembly);
        RegisterChromelyControllerAssembly(services, typeof(CommonController).Assembly);
    }

    public override void Configure(IConfigurationBuilder configBuilder)
    {
    }

    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        ErrorHandlingPath = "/Home/Error";

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                   Path.Combine(env.ContentRootPath, "assets")),
            RequestPath = "/assets"
        });


        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }

    public override void Configure(IWebHostBuilder builder)
    {
        builder
            .UseContentRoot(Directory.GetCurrentDirectory());
    }

    public override void Configure(IWebHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            services.InitializeDb();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}
