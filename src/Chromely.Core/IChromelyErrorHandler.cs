// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Net;
using Chromely.Core.Network;

namespace Chromely.Core
{
    public interface IChromelyErrorHandler
    {
        void HandleResourceError(string resourceStatus, string file, out HttpStatusCode statusCode, out string statusText);
        void HandleResourceError(string resourceStatus, string file, Exception exception, out HttpStatusCode statusCode, out string statusText);
        IChromelyResponse HandleRouteNotFound(string requestId, string routePath);
        IChromelyResponse HandleError(IChromelyRequest request, Exception exception);
        IChromelyResponse HandleError(IChromelyRequest request, IChromelyResponse response, Exception exception);
    }
}
