using Chromely.Core;
using Chromely.Core.Host;
using Chromely.Windows;

namespace Chromely
{
    /// <summary>
    /// Simplest Chromely app implementation.
    /// Be sure to call base implementations on derived implementations.
    /// </summary>
    public class ChromelyBasicApp: ChromelyApp
    {
        /// <summary>
        /// Configure IoC container contents.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(IChromelyContainer container)
        {
            base.Configure(container);
            container.RegisterByTypeSingleton(typeof(IChromelyWindow), typeof(ChromelyWindow));
        }

        /// <summary>
        /// Override to register Chromely events
        /// or use ChromelyEventedApp which already registers some events.
        /// </summary>
        /// <param name="container"></param>
        public override void RegisterEvents(IChromelyContainer container)
        {
        }

        /// <summary>
        /// Creates the main window.
        /// </summary>
        /// <returns></returns>
        public override IChromelyWindow CreateWindow()
        {
            return (IChromelyWindow)Container.GetInstance(typeof(IChromelyWindow), typeof(IChromelyWindow).Name);
        }
    }
}
