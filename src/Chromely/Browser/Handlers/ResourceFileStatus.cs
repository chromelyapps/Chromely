// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Net;

namespace Chromely.Browser
{
    public enum ResourceFileStatus
    {
        Ok,
        FileFound,
        ZeroFileSize,
        FileNotFound,
        FileProcessingError
    }

    public static class ResourceFileStatusExtension
    {
        public static (int, string) GetStatus(this ResourceFileStatus resFilestatus)
        {
           switch (resFilestatus)
            {
                case ResourceFileStatus.Ok:
                case ResourceFileStatus.FileFound:
                    return ((int)HttpStatusCode.OK, "OK");
                case ResourceFileStatus.ZeroFileSize:
                    return ((int)HttpStatusCode.BadRequest, "Resource loading error: file size is zero.");
                case ResourceFileStatus.FileNotFound:
                    return ((int)HttpStatusCode.NotFound, "File not found.");
                case ResourceFileStatus.FileProcessingError:
                    return ((int)HttpStatusCode.BadRequest, "Resource loading error.");
                default:
                    return ((int)HttpStatusCode.OK, "OK");
            }
        }
    }
}
