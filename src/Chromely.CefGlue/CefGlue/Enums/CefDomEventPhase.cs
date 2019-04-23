//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_dom_event_phase_t.
//
#pragma warning disable 1591
// ReSharper disable once CheckNamespace
namespace Xilium.CefGlue
{
    using System;

    /// <summary>
    /// DOM event processing phases.
    /// </summary>
    public enum CefDomEventPhase
    {
        Unknown = 0,
        Capturing,
        AtTarget,
        Bubbling,
    }
}
