// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public interface IChromelyResource
{
    MemoryStream Content { get; set; }
    string MimeType { get; set; }
    HttpStatusCode StatusCode { get; set; }
    string StatusText { get; set; }
    IDictionary<string, string[]> Headers { get; set; }
}