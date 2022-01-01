// Optional config
var config = DefaultConfiguration.CreateForRuntimePlatform();
config.StartUrl = "local://app/index.html";
//config.StartUrl = "local://app/index_draggable.html";

//config.StartUrl = "local://app/index_frameless.html";
//config.WindowOptions.WindowFrameless = true;


ThreadApt.STA();

AppBuilder
    .Create(args)
    .UseConfig<DefaultConfiguration>(config)
    .UseWindow<WindowSample>()
    .UseApp<AppSample>()
    .Build()
    .Run();