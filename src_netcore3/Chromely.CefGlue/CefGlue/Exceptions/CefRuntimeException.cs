#pragma warning disable 1591
// ReSharper disable once CheckNamespace
namespace Xilium.CefGlue
{
    using System;

    public class CefRuntimeException : Exception
    {
        public CefRuntimeException(string message)
            : base(message)
        {
        }
    }
}
