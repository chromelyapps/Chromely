namespace Chromely.Core.Network
{
    public interface IChromelyCommandTaskRunner
    {
        void Run(string url);
        void RunAsync(string url);
    }
}
