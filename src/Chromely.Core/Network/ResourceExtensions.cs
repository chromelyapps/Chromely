// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public static class ResourceExtensions
{
    public static MemoryStream GetMemoryStream(this string statusText)
    {
        if (string.IsNullOrWhiteSpace(statusText))
        {
            return new MemoryStream();
        }

        var preamble = Encoding.UTF8.GetPreamble();
        var bytes = Encoding.UTF8.GetBytes(statusText);

        var memoryStream = new MemoryStream(preamble.Length + bytes.Length);

        memoryStream.Write(preamble, 0, preamble.Length);
        memoryStream.Write(bytes, 0, bytes.Length);

        memoryStream.Position = 0;

        return memoryStream;
    }
}