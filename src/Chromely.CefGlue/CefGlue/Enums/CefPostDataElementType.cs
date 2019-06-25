//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_postdataelement_type_t.
//
#pragma warning disable 1591
// ReSharper disable once CheckNamespace
namespace Xilium.CefGlue
{
    /// <summary>
    /// Post data elements may represent either bytes or files.
    /// </summary>
    public enum CefPostDataElementType
    {
        Empty = 0,
        Bytes,
        File,
    }
}
