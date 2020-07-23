// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

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