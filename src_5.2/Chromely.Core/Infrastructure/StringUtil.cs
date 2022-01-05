// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

#nullable enable
public static class StringUtil
{
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? data)
    {
        return string.IsNullOrEmpty(data);
    }

    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? data)
    {
        return string.IsNullOrWhiteSpace(data);
    }
}
#nullable restore


[AttributeUsage(AttributeTargets.Parameter)]
public sealed class NotNullWhenAttribute : Attribute
{
    public NotNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;
    public bool ReturnValue { get; }
}
