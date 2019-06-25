// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Method.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace Chromely.Core.RestfulService
{
    /// <summary>
    /// The method.
    /// </summary>
    public enum Method
    {
        /// <summary>
        /// The none.
        /// </summary>
        None,

        /// <summary>
        /// The get.
        /// </summary>
        GET,

        /// <summary>
        /// The post.
        /// </summary>
        POST,

        /// <summary>
        /// The put.
        /// </summary>
        PUT,

        /// <summary>
        /// The delete.
        /// </summary>
        DELETE,

        /// <summary>
        /// The head.
        /// </summary>
        HEAD,

        /// <summary>
        /// The options.
        /// </summary>
        OPTIONS,

        /// <summary>
        /// The patch.
        /// </summary>
        PATCH,

        /// <summary>
        /// The merge.
        /// </summary>
        MERGE
    }
}
