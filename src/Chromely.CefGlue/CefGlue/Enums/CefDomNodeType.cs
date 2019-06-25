//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_dom_node_type_t.
//
#pragma warning disable 1591
// ReSharper disable once CheckNamespace
namespace Xilium.CefGlue
{
    using System;

    /// <summary>
    /// DOM node types.
    /// </summary>
    public enum CefDomNodeType
    {
       Unsupported = 0,
       Element,
       Attribute,
       Text,
       CDataSection,
       ProcessingInstruction,
       Comment,
       Document,
       DocumentType,
       DocumentFragment,
    }
}
