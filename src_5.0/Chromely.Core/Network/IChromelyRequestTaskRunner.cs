using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chromely.Core.Network
{
    public interface IChromelyRequestTaskRunner
    {
        ChromelyResponse Run(ChromelyRequest request);
        ChromelyResponse Run(string method, string path, IDictionary<string, string> parameters, object postData);
        ChromelyResponse Run(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string requestData);
        Task<ChromelyResponse> RunAsync(ChromelyRequest request);
        Task<ChromelyResponse> RunAsync(string method, string path, IDictionary<string, string> parameters, object postData);
        Task<ChromelyResponse> RunAsync(string requestId, RoutePath routePath, IDictionary<string, string> parameters, object postData, string requestData);
    }
}
