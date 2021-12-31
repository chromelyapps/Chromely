// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

public static class LoggerExtensions
{
    public static void LogError(this ILogger logger, Exception exception)
    {
        logger.LogError(exception, exception?.Message);
    }
}