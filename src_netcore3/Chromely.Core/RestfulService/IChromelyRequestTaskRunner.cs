namespace Chromely.Core.RestfulService
{
    public interface IChromelyRequestTaskRunner
    {
        ChromelyResponse Run(ChromelyRequest request);
        ChromelyResponse Run(string method, string path, object parameters, object postData);
        ChromelyResponse Run(string requestId, RoutePath routePath, object parameters, object postData, string requestData);
    }
}
