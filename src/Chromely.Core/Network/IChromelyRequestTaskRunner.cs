// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chromely.Core.Network
{
    public interface IChromelyRequestTaskRunner
    {
        IChromelyResponse Run(IChromelyRequest request);
        IChromelyResponse Run(string path, IDictionary<string, string> parameters, object postData);
        IChromelyResponse Run(string requestId, string routeUrl, IDictionary<string, string> parameters, object postData, string requestData);
        Task<IChromelyResponse> RunAsync(IChromelyRequest request);
        Task<IChromelyResponse> RunAsync(string path, IDictionary<string, string> parameters, object postData);
        Task<IChromelyResponse> RunAsync(string requestId, string routeUrl, IDictionary<string, string> parameters, object postData, string requestData);
    }
}
