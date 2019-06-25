//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_v8_propertyattribute_t.
//
#pragma warning disable 1591
// ReSharper disable once CheckNamespace
namespace Xilium.CefGlue
{
    using System;

    /// <summary>
    /// V8 property attribute values.
    /// </summary>
    [Flags]
    public enum CefV8PropertyAttribute
    {
        None = 0,
        ReadOnly = 1 << 0,
        DontEnum = 1 << 1,
        DontDelete = 1 << 2,
    }
}
