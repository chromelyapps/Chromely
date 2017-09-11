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

namespace Chromely.Core.RestfulService
{
    using LitJson;

    public class ChromelyResponse
    {
        private string m_jsonData;
        private object m_data;

        public ChromelyResponse()
        {
            m_jsonData = null;
            m_data = null;
        }

        public bool UseJsonData
        {
            get
            {
                if ((m_data == null) && !string.IsNullOrEmpty(m_jsonData))
                {
                    return true;
                }

                return false;
            }
        }

        public object Data
        {
            get
            {
                return m_data;
            }
            set
            {
                m_jsonData = null;
                m_data = value;
            }
        }

        public string JsonData
        {
            get
            {
                if (!string.IsNullOrEmpty(m_jsonData))
                {
                    return m_jsonData;
                }

                if (m_data != null)
                {
                    return JsonMapper.ToJson(m_data);
                }

                return null;
            }
            set
            {
                m_jsonData = value;
            }
        }
    }
}
