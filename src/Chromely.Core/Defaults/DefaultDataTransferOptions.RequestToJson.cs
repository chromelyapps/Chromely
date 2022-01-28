// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Defaults;

public partial class DataTransferOptions
{
    /// <inheritdoc />
    public virtual string? ConvertRequestToJson(object request)
    {
        if (request.IsValidJson())
        {
            return request.ToString();
        }

        var outStream = request as Stream;

        // Convert stream to Json
        if (outStream is not null)
        {
            using StreamReader reader = new(outStream);
            return reader.ReadToEnd();
        }

        // Default option
        // We should not get here ..
        return request.ToString();
    }
}