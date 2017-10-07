namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// Structure representing geoposition information. The properties of this
    /// structure correspond to those of the JavaScript Position object although
    /// their types may differ.
    /// </summary>
    public unsafe sealed class CefGeoposition
    {
        internal static CefGeoposition FromNative(cef_geoposition_t* position)
        {
            return new CefGeoposition(position);
        }

        private readonly double _latitude;
        private readonly double _longitude;
        private readonly double _altitude;
        private readonly double _accuracy;
        private readonly double _altitudeAccuracy;
        private readonly double _heading;
        private readonly double _speed;
        private readonly DateTime _timestamp;
        private readonly CefGeopositionErrorCode _errorCode;
        private readonly string _errorMessage;

        private CefGeoposition(cef_geoposition_t* position)
        {
            _latitude = position->latitude;
            _longitude = position->longitude;
            _altitude = position->altitude;
            _accuracy = position->accuracy;
            _altitudeAccuracy = position->altitude_accuracy;
            _heading = position->heading;
            _speed = position->speed;
            _timestamp = cef_time_t.ToDateTime(&position->timestamp);
            _errorCode = position->error_code;
            _errorMessage = cef_string_t.ToString(&position->error_message);
        }

        /// <summary>
        /// Latitude in decimal degrees north (WGS84 coordinate frame).
        /// </summary>
        public double Latitude { get { return _latitude; } }

        /// <summary>
        /// Longitude in decimal degrees west (WGS84 coordinate frame).
        /// </summary>
        public double Longitude { get { return _longitude; } }

        /// <summary>
        /// Altitude in meters (above WGS84 datum).
        /// </summary>
        public double Altitude { get { return _altitude; } }

        /// <summary>
        /// Accuracy of horizontal position in meters.
        /// </summary>
        public double Accuracy { get { return _accuracy; } }

        /// <summary>
        /// Accuracy of altitude in meters.
        /// </summary>
        public double AltitudeAccuracy { get { return _altitudeAccuracy; } }

        /// <summary>
        /// Heading in decimal degrees clockwise from true north.
        /// </summary>
        public double Heading { get { return _heading; } }

        /// <summary>
        /// Horizontal component of device velocity in meters per second.
        /// </summary>
        public double Speed { get { return _speed; } }

        /// <summary>
        /// Time of position measurement in milliseconds since Epoch in UTC time. This
        /// is taken from the host computer's system clock.
        /// </summary>
        public DateTime Timestamp { get { return _timestamp; } }

        /// <summary>
        /// Error code, see enum above.
        /// </summary>
        public CefGeopositionErrorCode ErrorCode { get { return _errorCode; } }

        /// <summary>
        /// Human-readable error message.
        /// </summary>
        public string ErrorMessage { get { return _errorMessage; } }
    }
}
