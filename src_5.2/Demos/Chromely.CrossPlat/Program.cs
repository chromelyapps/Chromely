// Optional config
var config = DefaultConfiguration.CreateForRuntimePlatform();
config.StartUrl = "local://app/index.html";

ThreadApt.STA();

AppBuilder
    .Create(args)
    .UseConfig<DefaultConfiguration>(config)
    .UseWindow<WindowSample>()
    .UseApp<AppSample>()
    .Build()
    .Run();