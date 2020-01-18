// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleLogger.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.Core.Logging
{
    [Flags]
    // ReSharper disable once StyleCop.SA1201
    internal enum LogLevel
    {
        TRACE,
        INFO,
        VERBOSE,
        DEBUG,
        WARN,
        ERROR,
        FATAL,
        CRITICAL
    }
}