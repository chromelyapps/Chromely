// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser
{
    using System;
    using System.Threading.Tasks;

    public static class HandlerBaseExtension
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
