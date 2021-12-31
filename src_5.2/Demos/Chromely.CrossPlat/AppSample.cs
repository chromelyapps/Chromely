namespace Chromely.CrossPlat;

internal class AppSample : ChromelyBasicApp
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddLogging(configure => configure.AddConsole());
        services.AddLogging(configure => configure.AddFile("Logs/serilog-{Date}.txt"));

        /*
        // Optional - adding custom handler
        services.AddSingleton<CefDragHandler, CustomDragHandler>();
        */

        /*
        // Optional- using config section to register IChromelyConfiguration
        // This just shows how it can be used, developers can use custom classes to override this approach
        //
        var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
        var configuration = builder.Build();
        var config = DefaultConfiguration.CreateFromConfigSection(configuration);
        services.AddSingleton<IChromelyConfiguration>(config);
        */

        /* Optional
        var options = new JsonSerializerOptions();
        options.ReadCommentHandling = JsonCommentHandling.Skip;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.AllowTrailingCommas = true;
        services.AddSingleton<JsonSerializerOptions>(options);
        */

        RegisterChromelyControllerAssembly(services, typeof(AppSample).Assembly);
    }
}