using Chromely.Core.Network;
using System;

namespace Chromely.Core.Host
{
    public interface IChromelyWindow : IChromelyServiceProvider, IDisposable
    {
        IntPtr Handle { get; }
        object Browser { get; }
        int Run(string[] args);
        void Close();
        void Exit();
    }
}