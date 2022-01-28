// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

/// <summary>
/// Resource response class.
/// </summary>
public interface IChromelyResource
{
    /// <summary>
    /// Gets or sets the response content data.
    /// </summary>
    MemoryStream? Content { get; set; }

    /// <summary>
    /// Gets or sets the mime type.
    /// </summary>
    string MimeType { get; set; }

    /// <summary>
    /// Gets or sets the response status code.
    /// </summary>
    HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the response status text.
    /// </summary>
    string StatusText { get; set; }

    /// <summary>
    /// Gets or sets the response headers.
    /// </summary>
    IDictionary<string, string[]> Headers { get; set; }
}