var config = DefaultConfiguration.CreateForRuntimePlatform();
config.StartUrl = "http://chromely.owin.com/";

ThreadApt.STA();

OwinAppBuilder
    .Create(args)
    .UseConfig<DefaultConfiguration>(config)
    .UseWindow<OwinWindowSample>()
    .UseApp<OwinAppSample>()
    .Build()
    .Run();
