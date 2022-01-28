// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.


using System.IO;
using System.Net;

namespace Chromely.Core.Network
{
    public interface IChromelyResource
    {
        MemoryStream Content { get; set; }
        string MimeType { get; set; }
        HttpStatusCode StatusCode { get; set; }
        string StatusText { get; set; }
    }
}
