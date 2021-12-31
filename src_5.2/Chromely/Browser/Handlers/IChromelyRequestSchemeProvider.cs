// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Network;
using System.Collections.Generic;

namespace Chromely.Browser
{
    public interface IChromelyRequestSchemeProvider
    {
        void Add(UrlScheme scheme);
        UrlScheme GetScheme(string url);
        List<UrlScheme> GetAllSchemes();
        bool IsSchemeRegistered(string url);
    }
}
