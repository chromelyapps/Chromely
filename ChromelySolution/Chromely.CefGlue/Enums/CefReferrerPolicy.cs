//
// This file manually written from cef/include/internal/cef_types.h.
// C API name: cef_referrer_policy_t.
//
namespace Xilium.CefGlue
{
    /// <summary>
    /// Policy for how the Referrer HTTP header value will be sent during navigation.
    /// If the `--no-referrers` command-line flag is specified then the policy value
    /// will be ignored and the Referrer value will never be sent.
    /// </summary>
    public enum CefReferrerPolicy
    {
        /// <summary>
        /// Always send the complete Referrer value.
        /// </summary>
        Always,

        /// <summary>
        /// Use the default policy. This is REFERRER_POLICY_ORIGIN_WHEN_CROSS_ORIGIN
        /// when the `--reduced-referrer-granularity` command-line flag is specified
        /// and REFERRER_POLICY_NO_REFERRER_WHEN_DOWNGRADE otherwise.
        /// </summary>
        Default,

        /// <summary>
        /// When navigating from HTTPS to HTTP do not send the Referrer value.
        /// Otherwise, send the complete Referrer value.
        /// </summary>
        NoReferrerWhenDowngrade,

        /// <summary>
        /// Never send the Referrer value.
        /// </summary>
        Never,

        /// <summary>
        /// Only send the origin component of the Referrer value.
        /// </summary>
        Origin,

        /// <summary>
        /// When navigating cross-origin only send the origin component of the Referrer
        /// value. Otherwise, send the complete Referrer value.
        /// </summary>
        OriginWhenCrossOrigin,
    }
}
