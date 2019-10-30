namespace Chromely.Core.RestfulService
{
    public interface IChromelyCommandTaskRunner
    {
        void Run(string url);
        void RunAsync(string url);
    }
}
