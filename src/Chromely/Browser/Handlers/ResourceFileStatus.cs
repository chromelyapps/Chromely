// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.IO;
using System.Text;

namespace Chromely.Browser
{
    public static class ResourceFileStatusExtension
    {
        public static byte[] GetMemoryStream(this string statusText)
        {
            if (string.IsNullOrWhiteSpace(statusText))
            {
                return null;
            }

            var preamble = Encoding.UTF8.GetPreamble();
            var bytes = Encoding.UTF8.GetBytes(statusText);

            var memoryStream = new MemoryStream(preamble.Length + bytes.Length);

            memoryStream.Write(preamble, 0, preamble.Length);
            memoryStream.Write(bytes, 0, bytes.Length);

            memoryStream.Position = 0;

            return memoryStream.ToArray();
        }
    }
}
