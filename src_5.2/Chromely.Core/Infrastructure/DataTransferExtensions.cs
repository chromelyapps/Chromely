// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

public static class DataTransferExtensions
{
    public static bool IsValidJson(this object value)
    {
        try
        {
            return JsonDocument.Parse(value.ToString()) != null;
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
        Type subType = type.GetElementType();
        if (subType != null)
        {
            return subType;
        }

        subType = type.GenericTypeArguments[0];
        if (subType != null)
        {
            return subType;
        }

        return typeof(Object);
    }

    public static Type DictionaryElementKeyType(this Type type)
    {
        Type subType = type.GetElementType();
        if (subType != null)
        {
            return subType;
        }

        subType = type.GenericTypeArguments[0];
        if (subType != null)
        {
            return subType;
        }

        return typeof(Object);
    }

    public static Type DictionaryElementValueType(this Type type)
    {
        Type subType = type.GetElementType();
        if (subType != null)
        {
            return subType;
        }

        subType = type.GenericTypeArguments[1];
        if (subType != null)
        {
            return subType;
        }

        return typeof(Object);
    }

    public static IList CreateGenericList(this Type subType)
    {
        var listType = typeof(List<>);
        var constructedListType = listType.MakeGenericType(subType);
        var instance = Activator.CreateInstance(constructedListType);
        return (IList)instance;
    }

    public static object ChangeObjectType(this object value, Type type)
    {
        try
        {
            var isNullable = (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ||
                                              type.Name.Equals("Nullable`1") || type == typeof(Nullable<>) || type == typeof(Nullable));
            if (isNullable)
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (value == null)
            {
                if (isNullable)
                {
                    return null;
                }

                return type.DefaultValue();
            }

            return Convert.ChangeType(value, type);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return value;
    }

    public static object DefaultValue(this Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        return null;
    }

    public static JsonSerializerOptions ToSerializerOptions(this object serializerOption)
    {
        var options = serializerOption as JsonSerializerOptions;
        if (options != null)
        {
            return options;
        }

        options = new JsonSerializerOptions();
        options.ReadCommentHandling = JsonCommentHandling.Skip;
        options.AllowTrailingCommas = true;
        options.PropertyNameCaseInsensitive = true;
        return options;
    }

    public static JsonDocumentOptions DocumentOptions(this object serializerOption)
    {
        var options = serializerOption.ToSerializerOptions();
        var jsonDocumentOptions = new JsonDocumentOptions();
        jsonDocumentOptions.CommentHandling = options.ReadCommentHandling;
        jsonDocumentOptions.AllowTrailingCommas = options.AllowTrailingCommas;
        jsonDocumentOptions.MaxDepth = options.MaxDepth;

        return jsonDocumentOptions;
    }
}