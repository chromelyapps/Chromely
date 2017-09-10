//
// This file manually written from cef/include/internal/cef_types.h.
//
namespace Xilium.CefGlue.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    internal unsafe struct cef_geoposition_t
    {
        public double latitude;
        public double longitude;
        public double altitude;
        public double accuracy;
        public double altitude_accuracy;
        public double heading;
        public double speed;
        public cef_time_t timestamp;
        public CefGeopositionErrorCode error_code;
        public cef_string_t error_message;
    }
}
