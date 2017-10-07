//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_handler_errorcode_t.
//
namespace Xilium.CefGlue
{
    /// <summary>
    /// Supported error code values.
    /// </summary>
    public enum CefErrorCode
    {
        None = 0,

        Failed = -2,
        Aborted = -3,
        InvalidArgument = -4,
        InvalidHandle = -5,
        FileNotFound = -6,
        TimedOut = -7,
        FileTooBig = -8,
        Unexpected = -9,
        AccessDenied = -10,
        NotImplemented = -11,

        ConnectionClosed = -100,
        ConnectionReset = -101,
        ConnectionRefused = -102,
        ConnectionAborted = -103,
        ConnectionFailed = -104,
        NameNotResolved = -105,
        InternetDisconnected = -106,
        SslProtocolError = -107,
        AddressInvalid = -108,
        AddressUnreachable = -109,
        SslClientAuthCertNeeded = -110,
        TunnelConnectionFailed = -111,
        NoSslVersionsEnabled = -112,
        SslVersionOrCipherMismatch = -113,
        SslRenegotiationRequested = -114,

        CertBegin = CertCommonNameInvalid,
        CertCommonNameInvalid = -200,
        CertDateInvalid = -201,
        CertAuthorityInvalid = -202,
        CertContainsErrors = -203,
        CertNoRevocationMechanism = -204,
        CertUnableToCheckRevocation = -205,
        CertRevoked = -206,
        CertInvalid = -207,
        CertWeakSignatureAlgorithm = -208,
        // -209 is available: was ERR_CERT_NOT_IN_DNS
        CertNonUniqueName = -210,
        CertWeakKey = -211,
        CertNameConstraintViolation = -212,
        CertValidityTooLong = -213,
        CertEnd = CertValidityTooLong,

        InvalidUrl = -300,
        DisallowedUrlScheme = -301,
        UnknownUrlScheme = -302,
        TooManyRedirects = -310,
        UnsafeRedirect = -311,
        UnsafePort = -312,
        InvalidResponse = -320,
        InvalidChunkedEncoding = -321,
        MethodNotSupported = -322,
        UnexpectedProxyAuth = -323,
        EmptyResponse = -324,
        ResponseHeadersTooBig = -325,

        CacheMiss = -400,

        InsecureResponse = -501,
    }
}
