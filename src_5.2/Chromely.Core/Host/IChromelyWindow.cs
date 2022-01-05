// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Host;

public interface IChromelyWindow : IDisposable
{
    IntPtr Handle { get; }
    IChromelyNativeHost NativeHost { get; }
    void Init(object settings);
    void Create(IntPtr hostHandle, IntPtr winXID);
    void Close();
    void SetTitle(string title);
    void Resize(int width, int height);
    void RegisterHandlers();
    void NotifyOnMove();
    void Minimize();
    void Maximize();
    void Restore();
}