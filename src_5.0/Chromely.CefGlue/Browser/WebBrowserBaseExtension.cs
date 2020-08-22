// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebBrowserBaseExtension.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Browser
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The web browser base extension.
    /// </summary>
    public static class WebBrowserBaseExtension
    {
        /// <summary>
        /// The invoke async if possible.
        /// Executes the action asynchronously on the UI thread if possible. Does not block execution on the calling thread.
        /// </summary>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        public static void InvokeAsyncIfPossible(this object handler, Action action)
        {
            var task = new Task(action);
            task.ContinueWith(r =>
            {
                switch (task.Status)
                {
                    case TaskStatus.Canceled:
                        break;
                    case TaskStatus.Faulted:
                        action.Invoke();
                        break;
                    case TaskStatus.RanToCompletion:
                        break;
                }
            });

            task.Start();
        }
    }
}
