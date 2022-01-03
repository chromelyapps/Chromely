using Chromely.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Chromely.Core.Network
{
    public class ChromelyResource : IChromelyResource
    {
        public ChromelyResource()
        {
            StatusCode = ResourceConstants.StatusOK;
            StatusText = ResourceConstants.StatusOKText;
            MimeType = "text/plain";
            Headers = new Dictionary<string, string[]>();
        }

        public MemoryStream? Content { get; set; }
        public string MimeType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusText { get; set; }
        public IDictionary<string, string[]> Headers { get; set; }
    }
}
