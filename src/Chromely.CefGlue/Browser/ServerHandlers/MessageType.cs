// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageType.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Browser.ServerHandlers
{
    /// <summary>
    /// The message type.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The none.
        /// </summary>
        None,

        /// <summary>
        /// The echo.
        /// </summary>
        Echo,

        /// <summary>
        /// The target recepient.
        /// </summary>
        TargetRecepient,

        /// <summary>
        /// The broadcast.
        /// </summary>
        Broadcast,

        /// <summary>
        /// The controller action.
        /// </summary>
        ControllerAction
    }
}
