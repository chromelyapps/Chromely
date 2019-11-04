//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_value_type_t.
//
#pragma warning disable 1591
// ReSharper disable once CheckNamespace
namespace Xilium.CefGlue
{
    /// <summary>
    /// Supported value types.
    /// </summary>
    public enum CefValueType
    {
        Invalid = 0,
        Null,
        Bool,
        Int,
        Double,
        String,
        Binary,
        Dictionary,
        List,
    }
}
