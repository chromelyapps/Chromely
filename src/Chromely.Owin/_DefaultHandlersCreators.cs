// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Owin;

internal sealed class DefaultOwinSchemeHandlerFactory : OwinSchemeHandlerFactory, IDefaultOwinCustomHandler
{
    public DefaultOwinSchemeHandlerFactory(IOwinPipeline owinPipeline, IChromelyErrorHandler errorHandler) : base(owinPipeline, errorHandler)
    {
    }
}