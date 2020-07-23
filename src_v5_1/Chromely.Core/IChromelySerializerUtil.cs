// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Text.Json;

namespace Chromely.Core
{
    public interface IChromelySerializerUtil
    {
        JsonSerializerOptions SerializerOptions { get; set; }
        string ObjectToJson(object value);
        string EnsureResponseDataIsJson(object value);
        IDictionary<string, object> JsonToArray(string json);
        IDictionary<string, string> ObjectToDictionary(object value);
    }
}
