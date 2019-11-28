using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Windows;

namespace Chromely
{
    public abstract class BasicChromelyApp: ChromelyApp
    {
        public override void Configure(IChromelyContainer container)
        {
            base.Configure(container);
            container.RegisterSingleton(typeof(IChromelyWindow), typeof(IChromelyWindow).Name, typeof(ChromelyWindow));
        }

        public override void RegisterEvents(IChromelyContainer container)
        {
        }

        public override IChromelyWindow CreateWindow()
        {
            return (IChromelyWindow)Container.GetInstance(typeof(IChromelyWindow), typeof(IChromelyWindow).Name);
        }
    }
}
