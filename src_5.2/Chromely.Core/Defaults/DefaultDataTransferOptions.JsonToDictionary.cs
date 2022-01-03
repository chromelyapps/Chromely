// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Defaults;

public partial class DataTransferOptions
{
    public virtual object? ConvertJsonToDictionary(string json, Type typeToConvert)
    {
        if (!typeToConvert.IsDictionaryType())
        {
            return default;
        }

        var keyType = typeToConvert.GetGenericArguments()[0];
        var valueType = typeToConvert.GetGenericArguments()[1];

        return Convert(json, keyType, valueType);
    }

    private object? Convert(string json, Type keyType, Type valueType)
    {
        Type[] templateTypes = new Type[] { keyType, valueType };
        Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(templateTypes);
        var valueDic = Activator.CreateInstance(dictionaryType);

        if (valueDic is not null)
        {
            var jsonDocumentOptions = new JsonDocumentOptions
            {
                CommentHandling = Options.ReadCommentHandling,
                AllowTrailingCommas = Options.AllowTrailingCommas,
                MaxDepth = Options.MaxDepth
            };

            var dicProp = valueDic.GetType().GetProperty("Item");

            using JsonDocument jsonDocument = JsonDocument.Parse(json, jsonDocumentOptions);
            foreach (JsonProperty element in jsonDocument.RootElement.EnumerateObject())
            {
                var key = ExtractKey(element.Name, keyType);
                var value = ExtractValue(element.Value, valueType);

                dicProp?.SetValue(valueDic, value, new[] { key });
            }
        }

        return valueDic;
    }

    private object? ExtractKey(string keyString, Type keyType)
    {
        if (string.IsNullOrWhiteSpace(keyString))
        {
            return keyType.DefaultValue();
        }

        else if (keyType.IsValueType || (keyType == typeof(string)))
        {
            try
            {
                return System.Convert.ChangeType(keyString, keyType);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception);
            }
        }

        else
        {
            try
            {
                return JsonSerializer.Deserialize(keyString, keyType, Options);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception);
            }
        }

        return default;
    }

    private object? ExtractValue(JsonElement value, Type type)
    {
        try
        {
            TypeCode typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return type.DefaultValue();

                case TypeCode.Boolean:
                    return value.GetBoolean();

                case TypeCode.Char:
                    {
                        var tempData = value.GetString();
                        if (tempData is not null)
                        {
                            return tempData[0];
                        }

                        return char.MinValue;
                    }

                case TypeCode.SByte:
                    return value.GetSByte();

                case TypeCode.Byte:
                    return value.GetByte();

                case TypeCode.Int16:
                    return value.GetInt16();

                case TypeCode.UInt16:
                    return value.GetUInt16();

                case TypeCode.Int32:
                    return value.GetInt32();

                case TypeCode.UInt32:
                    return value.GetUInt32();

                case TypeCode.Int64:
                    return value.GetInt64();

                case TypeCode.UInt64:
                    return value.GetUInt64();

                case TypeCode.Single:
                    return value.GetSingle();

                case TypeCode.Double:
                    return value.GetDouble();

                case TypeCode.Decimal:
                    return value.GetDecimal();

                case TypeCode.DateTime:
                    return value.GetDateTime();

                case TypeCode.String:
                    return value.GetString();

                case TypeCode.Object:
                    {
                        if (type.IsGuidtype())
                        {
                            return value.GetGuid();
                        }

                        if (type.IsDictionaryType())
                        {
                            var args = type.GetGenericArguments();

                            return Convert(value.GetRawText(), args[0], args[1]);
                        }

                        return JsonSerializer.Deserialize(value.GetRawText(), type, Options);
                    }

                default:
                    return JsonSerializer.Deserialize(value.GetRawText(), type, Options);
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return type.DefaultValue();
    }
}