// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

/// <summary>
/// Default implementation of <see cref="ChromelyResource"/>.
/// </summary>
public class ChromelyResource : IChromelyResource
{
    /// <summary>
    /// Initializes a new instance of <see cref="ChromelyResource"/>.
    /// </summary>
    public ChromelyResource()
    {
        StatusCode = ResourceConstants.StatusOK;
        StatusText = ResourceConstants.StatusOKText;
        MimeType = "text/plain";
        Headers = new Dictionary<string, string[]>();
    }

    /// <inheritdoc/>
    public MemoryStream? Content { get; set; }

    /// <inheritdoc/>
    public string MimeType { get; set; }

    /// <inheritdoc/>
    public HttpStatusCode StatusCode { get; set; }

    /// <inheritdoc/>
    public string StatusText { get; set; }

    /// <inheritdoc/>
    public IDictionary<string, string[]> Headers { get; set; }
}