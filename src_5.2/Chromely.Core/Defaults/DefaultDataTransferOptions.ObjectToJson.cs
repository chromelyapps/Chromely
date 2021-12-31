// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Defaults;

public partial class DataTransferOptions
{
    public virtual string ConvertObjectToJson(object value)
    {
        try
        {
            if (value == null)
            {
                return null;
            }

            var stream = value as Stream;
            if (stream != null)
            {
                using (var reader = new StreamReader(stream, Encoding))
                {
                    value = reader.ReadToEnd();
                }
            }

            if (value.IsValidJson())
            {
                return value.ToString();
            }

            return JsonSerializer.Serialize(value, Options);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return value.ToString();
    }
}