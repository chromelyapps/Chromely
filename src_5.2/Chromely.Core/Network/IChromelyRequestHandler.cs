// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public interface IChromelyRequestHandler
{
    void Execute(string url);
    IChromelyResponse Execute(string requestId, string routeUrl, IDictionary<string, object> parameters, object postData, string requestData);
    Task<IChromelyResponse> ExecuteAsync(string requestId, string routeUrl, IDictionary<string, object> parameters, object postData, string requestData);
}