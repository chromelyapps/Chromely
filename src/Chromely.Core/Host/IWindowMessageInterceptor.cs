using System;

namespace Chromely.Core.Host
{
    public interface IWindowMessageInterceptor
    {
        void Setup(IChromelyNativeHost nativeHost, IntPtr browserHandle);
    }
}
