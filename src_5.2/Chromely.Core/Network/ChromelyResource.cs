// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public class ChromelyResource : IChromelyResource
{
    public ChromelyResource()
    {
        StatusCode = ResourceConstants.StatusOK;
        StatusText = ResourceConstants.StatusOKText;
        MimeType = "text/plain";
        Headers = new Dictionary<string, string[]>();
    }

    public MemoryStream? Content { get; set; }
    public string MimeType { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string StatusText { get; set; }
    public IDictionary<string, string[]> Headers { get; set; }
}