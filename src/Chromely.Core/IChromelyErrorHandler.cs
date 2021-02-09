// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Net;
using Chromely.Core.Network;

namespace Chromely.Core
{
    public interface IChromelyErrorHandler
    {
        IChromelyResponse HandleRouteNotFound(string requestId, string routePath);
        IChromelyResource HandleError(FileInfo fileInfo, Exception exception = null);
        IChromelyResource HandleError(Stream stream, Exception exception = null);
        IChromelyResponse HandleError(IChromelyRequest request, Exception exception);
        IChromelyResponse HandleError(IChromelyRequest request, IChromelyResponse response, Exception exception);
    }
}
