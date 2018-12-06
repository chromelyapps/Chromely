// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebBrowserBase.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Host
{
    using System;

    /// <summary>
    /// The web browser base.
    /// </summary>
    public abstract class WebBrowserBase : IDisposable
    {
        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool mDisposed;

        /// <summary>
        /// Finalizes an instance of the <see cref="WebBrowserBase"/> class. 
        /// </summary>
        ~WebBrowserBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// The dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The dispose method - checks if disposing flag is set.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (mDisposed)
            {
                return;
            }

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            mDisposed = true;
        }
    }
}
