// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.NativeHosts
{
    abstract partial class NativeHostBase
    {
        #region Disposal

        ~NativeHostBase()
        {
            Dispose(false);
        }

        private bool _disposed = false;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Flag indicating if managed resources should be disposed too. Yes, if true.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            // If there are managed resources
            if (disposing)
            {
            }

            DetachHooks();

            _disposed = true;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
