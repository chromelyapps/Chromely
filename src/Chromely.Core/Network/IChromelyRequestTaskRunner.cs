using System.Collections.Generic;

namespace Chromely.Core.Network
{
    public interface IChromelyRequestTaskRunner
    {
        ChromelyResponse Run(ChromelyRequest request);
        ChromelyResponse Run(string method, string path, IDictionary<string, string> parameters, object postData);
        ChromelyResponse Run(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string requestData);
    }
}
