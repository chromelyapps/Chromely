using System;

namespace Chromely.Core.Host
{
    public interface IFramelessWindowService
    {
        IntPtr Handle { get; }
        void Close();
        bool Maximize();
        bool Minimize();
        bool Restore();
    }
}