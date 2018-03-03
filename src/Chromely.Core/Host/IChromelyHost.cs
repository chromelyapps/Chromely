namespace Chromely.Core.Host
{
    public interface IChromelyHost
    {
        ChromelyConfiguration HostConfig { get; }

        void RegisterSchemeHandlers();
        void RegisterMessageRouters();
    }
}
