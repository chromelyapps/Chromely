/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.Core.Infrastructure
{
    using System;

    public class UrlScheme
    {
        public UrlScheme(string scheme, string host, bool isExternal)
        {
            Scheme = scheme;
            Host = host;
            IsExternal = isExternal;
        }

        public UrlScheme(string url, bool isExternal)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var uri = new Uri(url);
                Scheme = uri.Scheme;
                Host = uri.Host;
                IsExternal = isExternal;
            }
        }

        public string Scheme { get; set; }
        public string Host { get; set; }
        public bool IsExternal { get; set; }

        public bool IsUrlOfSameScheme(string url)
        {
            if (string.IsNullOrEmpty(Scheme) ||
                string.IsNullOrEmpty(Host) ||
                string.IsNullOrEmpty(url))
            {
                return false;
            }

            var uri = new Uri(url);

            if (string.IsNullOrEmpty(uri.Scheme) ||
                string.IsNullOrEmpty(uri.Host))
            {
                return false;
            }

            if (Scheme.ToLower().Equals(uri.Scheme) &&
                Host.ToLower().Equals(uri.Host))
            {
                return true;
            }

            return false;
        }
    }
}
