// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CallbackResponseStruct.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi.Browser.Handlers
{
    /// <summary>
    /// The callback response struct.
    /// </summary>
    internal struct CallbackResponseStruct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackResponseStruct"/> struct.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        public CallbackResponseStruct(string response)
        {
            ResponseText = response;
        }

        /// <summary>
        /// Gets or sets the response text.
        /// </summary>
        public string ResponseText { get; set; }
    }
}
