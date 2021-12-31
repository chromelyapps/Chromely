// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using Xilium.CefGlue;

namespace Chromely
{
    /// <summary>
    /// The action task.
    /// </summary>
    internal sealed class ActionTask : CefTask
    {
        internal static void PostTask(CefThreadId threadId, Action action)
        {
            CefRuntime.PostTask(threadId, new ActionTask(action));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTask"/> class.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public ActionTask(Action action)
        {
            Action = action;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        protected override void Execute()
        {
            Action();
            Action = null;
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        private Action Action { get; set; }
    }

}
