// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Defaults;

public partial class DataTransferOptions
{
    /// <inheritdoc />
    public virtual object? ConvertJsonToObject(string json, Type typeToConvert)
    {
        try
        {
            if (typeof(Object) == typeToConvert)
            {
                return ConvertJsonToDynamic(json);
            }

            return JsonSerializer.Deserialize(json, typeToConvert, Options);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return typeToConvert.DefaultValue();
    }

    private dynamic? ConvertJsonToDynamic(string json)
    {
        try
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json, Options);
            return ConvertJsonElementToDynamic(jsonElement);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
            Logger.Instance.Log.LogWarning("If this is about cycle was detection please see - https://github.com/dotnet/corefx/issues/41288");
        }

        return default;
    }

    private dynamic ConvertJsonElementToDynamic(JsonElement jsonElement)
    {
        var dic = new Dictionary<string, object>();

        foreach (var jsonProperty in jsonElement.EnumerateObject())
        {
            switch (jsonProperty.Value.ValueKind)
            {
                case JsonValueKind.Null:
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    dic.Add(jsonProperty.Name, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                    break;

                case JsonValueKind.Number:
                    dic.Add(jsonProperty.Name, jsonProperty.Value.GetDouble());
                    break;

                case JsonValueKind.False:
                    dic.Add(jsonProperty.Name, false);
                    break;

                case JsonValueKind.True:
                    dic.Add(jsonProperty.Name, true);
                    break;

                case JsonValueKind.Undefined:
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    dic.Add(jsonProperty.Name, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                    break;

                case JsonValueKind.String:
                    var strValue = jsonProperty.Value.GetString();
                    if (DateTime.TryParse(strValue, out DateTime date))
                    {
                        dic.Add(jsonProperty.Name, date);
                    }
                    else
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        dic.Add(jsonProperty.Name, strValue);
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                    break;

                case JsonValueKind.Object:
                    dic.Add(jsonProperty.Name, ConvertJsonElementToDynamic(jsonProperty.Value));
                    break;

                case JsonValueKind.Array:
                    ArrayList objectList = new();
                    foreach (JsonElement item in jsonProperty.Value.EnumerateArray())
                    {
                        switch (item.ValueKind)
                        {
                            case JsonValueKind.Null:
                                objectList.Add(null);
                                break;

                            case JsonValueKind.Number:
                                objectList.Add(item.GetDouble());
                                break;

                            case JsonValueKind.False:
                                objectList.Add(false);
                                break;

                            case JsonValueKind.True:
                                objectList.Add(true);
                                break;

                            case JsonValueKind.Undefined:
                                objectList.Add(null);
                                break;

                            case JsonValueKind.String:
                                var itemValue = item.GetString();
                                if (DateTime.TryParse(itemValue, out DateTime itemDate))
                                {
                                    objectList.Add(itemDate);
                                }
                                else
                                {
                                    objectList.Add(itemValue);
                                }
                                break;

                            default:
                                objectList.Add(ConvertJsonElementToDynamic(item));
                                break;
                        }
                    }

                    dic.Add(jsonProperty.Name, objectList);
                    break;
            }
        }

        return dic.Aggregate(new ExpandoObject() as IDictionary<string, Object>, (a, p) => { a.Add(p.Key, p.Value); return a; });
    }
}