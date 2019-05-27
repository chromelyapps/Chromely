// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogSeverity.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Infrastructure
{
    /// <summary>
    /// The log severity.
    /// </summary>
    public enum LogSeverity
    {
        /// <summary>
        /// The default.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The verbose.
        /// </summary>
        Verbose = 1,

        /// <summary>
        /// The info.
        /// </summary>
        Info = 2,

        /// <summary>
        /// The warning.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// The error.
        /// </summary>
        Error = 4,

        /// <summary>
        /// The error report.
        /// </summary>
        ErrorReport = 5,

        /// <summary>
        /// The disable.
        /// </summary>
        Disable = 99
    }
}
