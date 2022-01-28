// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

public static class DataTransferExtensions
{
    public static bool IsValidJson(this object value)
    {
        try
        {
            if (value is null)
            {
                return false;
            }

            var tempData = value.ToString();
            if (tempData is null)
            {
                return false;
            }

            return  JsonDocument.Parse(tempData) is not null;
        }
        catch { }

        return false;
    }

    public static bool IsOfType<T>(object value)
    {
        return value is T;
    }

    public static bool IsGuidtype(this Type type)
    {
        if (type == typeof(Guid) || type == typeof(Guid?))
        {
            return true;
        }

        return false;
    }

    public static bool IsArrayType(this Type type)
    {
        return type.IsArray ||
               type.Name == "IList`1" ||
               type.Name == "List`1" ||
               type == typeof(IList) ||
               type == typeof(IList<>) ||
               type == typeof(List<object>) ||
               typeof(Array).IsAssignableFrom(type) ||
               typeof(List<object>).IsAssignableFrom(type);
    }

    public static bool IsDictionaryType(this Type type)
    {
        return type.Name.Equals("IDictionary`2") ||
               (type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Dictionary<,>));
    }

    public static Type ArrayElementType(this Type type)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Type subType = type.GetElementType();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        if (subType is not null)
        {
            return subType;
        }

        subType = type.GenericTypeArguments[0];
        if (subType is not null)
        {
            return subType;
        }

        return typeof(Object);
    }

    public static Type DictionaryElementKeyType(this Type type)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Type subType = type.GetElementType();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        if (subType is not null)
        {
            return subType;
        }

        subType = type.GenericTypeArguments[0];
        if (subType is not null)
        {
            return subType;
        }

        return typeof(Object);
    }

    public static Type DictionaryElementValueType(this Type type)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Type subType = type.GetElementType();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        if (subType is not null)
        {
            return subType;
        }

        subType = type.GenericTypeArguments[1];
        if (subType is not null)
        {
            return subType;
        }

        return typeof(Object);
    }

    public static IList? CreateGenericList(this Type subType)
    {
        var listType = typeof(List<>);
        var constructedListType = listType.MakeGenericType(subType);
        var instance = Activator.CreateInstance(constructedListType);
        return instance as IList;
    }

    public static object? ChangeObjectType(this object value, Type type)
    {
        try
        {
            var isNullable = (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ||
                                              type.Name.Equals("Nullable`1") || type == typeof(Nullable<>) || type == typeof(Nullable));
            if (isNullable)
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                type = Nullable.GetUnderlyingType(type);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }

            if (value is null)
            {
                if (isNullable)
                {
                    return default;
                }

                return type?.DefaultValue();
            }

#pragma warning disable CS8604 // Possible null reference argument.
            return Convert.ChangeType(value, type);
#pragma warning restore CS8604 // Possible null reference argument.
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return value;
    }

    public static object? DefaultValue(this Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        return default;
    }

    public static JsonSerializerOptions ToSerializerOptions(this object serializerOption)
    {
        var options = serializerOption as JsonSerializerOptions;
        if (options is not null)
        {
            return options;
        }

        options = new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true
        };
        return options;
    }

    public static JsonDocumentOptions DocumentOptions(this object serializerOption)
    {
        var options = serializerOption.ToSerializerOptions();
        var jsonDocumentOptions = new JsonDocumentOptions
        {
            CommentHandling = options.ReadCommentHandling,
            AllowTrailingCommas = options.AllowTrailingCommas,
            MaxDepth = options.MaxDepth
        };

        return jsonDocumentOptions;
    }
}