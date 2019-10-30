using System;
using Chromely;
using Chromely.Core;
using Chromely.Core.RestfulService;
using CrossPlatDemo.Controllers;

namespace NetFrameworkDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            AppBuilder
               .Create()
               .UseApp<DemoChromelyApp>()
               .Build()
               .Run(args);
        }
    }

    public class DemoChromelyApp : BasicChromelyApp
    {
        public override void Configure(IChromelyContainer container)
        {
            base.Configure(container);
            container.RegisterSingleton(typeof(ChromelyController), Guid.NewGuid().ToString(), typeof(DemoController));
            container.RegisterSingleton(typeof(ChromelyController), Guid.NewGuid().ToString(), typeof(ExecuteJavaScriptDemoController));
        }
    }
}