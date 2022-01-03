// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Defaults;

public partial class DataTransferOptions
{
    public virtual string? ConvertResponseToJson(object? response)
    {
        return ConvertObjectToJson(response);
    }
}