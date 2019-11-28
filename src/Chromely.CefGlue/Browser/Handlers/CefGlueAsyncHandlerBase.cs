// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefGlueResourceSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core.Infrastructure;
using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// The CefGlue resource scheme handler.
    /// </summary>
    public abstract class CefGlueAsyncHandlerBase : CefResourceHandler, IDisposable
    {
        private bool _disposed;
        private const int _bufferSize = 32 * 1024;

        private Stream _dataStream;
        private long _totalRead;
        private int _bytesRead = -1;

        private CancellationTokenSource _cancellationTokenSource;
        private byte[] _rentedBuffer;


        /// <summary>
        /// Initializes a new instance of the Chromely.CefGlue.Browser.Handlers.CefGlueResourceSchemeHandlerAsync class.
        /// </summary>
        public CefGlueAsyncHandlerBase()
        {
        }

        /// <summary>
        /// Finalizes object
        /// </summary>
        ~CefGlueAsyncHandlerBase()
        {
            Dispose(false);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            //breaking disposable pattern, because i'm not sure, if this method is used right
            //so i'm freening all resources without taking account disposing parameter

            //if (disposing)
            //{
            //here should be freed only managed resources
            //}
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            FreeResources();

            // Free any unmanaged objects here.
            //

            _disposed = true;
            base.Dispose(disposing);
        }


        /// <summary>
        /// Prepares cef web request. 
        /// </summary>
        /// <returns>If request should be handled, returns true, otherwize false.</returns>
        protected abstract bool PrepareRequest(CefRequest request);

        /// <summary>
        /// Called async after Request processed. Should load data required to populate headers and later data.
        /// </summary>
        /// <returns>Returns true, if data available, otherwize false.</returns>
        protected abstract Task<bool> LoadResourceData(CancellationToken cancellationToken);

        /// <summary>
        /// If data stream available returns it, otherwize returns null
        /// </summary>
        protected abstract Task<Stream> GetResourceDataStream(CancellationToken cancellationToken);

        /// <summary>
        /// Returns data total size in bytes if available, otherwize false
        /// </summary>
        protected abstract long GetDataSize();

        /// <inheritdoc/>
        [Obsolete]
        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            if (!this.PrepareRequest(request))
                return false;

            _cancellationTokenSource = new CancellationTokenSource();
            _dataStream = null;

            GetResource();

            return true;

            async void GetResource()
            {
                try
                {
                    if (await LoadResourceData(_cancellationTokenSource.Token))
                    {
                        _dataStream = await GetResourceDataStream(_cancellationTokenSource.Token);
                    }
                }
                catch (TaskCanceledException)
                {
                    Logger.Instance.Log.Info("The request was canceled.");
                }
                catch (Exception ex) when (ex.Message == "The request was aborted: The request was canceled.")
                {
                    Logger.Instance.Log.Info("The request was canceled.");
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log.Error(ex, "Exception thrown while loading resource");
                }
                finally
                {
                    callback.Continue();
                    callback.Dispose();
                }
            }
        }

        /// <inheritdoc/>
        [Obsolete]
        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            // if no data stream was specified, then nothing to read
            // and we should stop reading.
            // return false to indicate this.
            if (_dataStream == null)
            {
                bytesRead = 0;
                callback.Continue();
                callback.Dispose();
                return false;
            }

            // when this is second call of ReadResponse, _bytesRead will be set to non negative number
            if (_bytesRead >= 0)
            {
                WriteReadBytes(out bytesRead);
                callback.Dispose();
                return bytesRead != 0; //If read 0 bytes, then we should return in false
            }

            bytesRead = 0;
            _ = ReadDataAsync();

            return true;


            // Copies read bytes into response and return true, if data is completed, owtherwize returns false
            async Task ReadDataAsync()
            {
                try
                {
                    if (_rentedBuffer == null)
                    {
                        _rentedBuffer = ArrayPool<byte>.Shared.Rent(_bufferSize);
                    }
                    //await _semaphore.WaitAsync();
                    _bytesRead = await _dataStream.ReadAsync(_rentedBuffer, 0, Math.Min(bytesToRead, _bufferSize), _cancellationTokenSource.Token);
                    //_semaphore.Release();
                }
                catch (TaskCanceledException)
                {
                    FreeResources();
                    _bytesRead = 0;
                    Logger.Instance.Log.Warn("Cancellation requested");
                }
                // Sometimes cef disposes object, before it is cancelled
                // then this exception is thrown
                // and i don't know how to fix this :(
                catch (ObjectDisposedException)
                {
                    FreeResources();
                    _bytesRead = 0;
                    Logger.Instance.Log.Warn("Cancellation requested");
                }
                catch (Exception ex)
                {
                    FreeResources();
                    _bytesRead = 0;

                    Logger.Instance.Log.Error(ex, "Exception thrown while loading resource");
                }
                finally
                {
                    callback.Continue();
                    callback.Dispose();
                }
            }

            bool WriteReadBytes(out int bytesAlreadyRead)
            {
                if (_bytesRead == 0) //if 0 bytes read, then no data was read and we should complete request
                {
                    bytesAlreadyRead = 0;
                    this.FreeResources();
                    return true;
                }

                if (_bytesRead > 0) //writing read bytes into a buffer if there was something
                    response.Write(_rentedBuffer, 0, _bytesRead);

                bytesAlreadyRead = _bytesRead; //incrementing counters
                _totalRead += _bytesRead;
                // Setting read bytes count to -1 to indicate, that nothing left to write
                // End on next call Read from source should be performed
                _bytesRead = -1;

                var dataSize = this.GetDataSize();

                // If datasize is known and we already read that amount, then stoping
                bool completed = dataSize >= 0 && _totalRead == dataSize;
                // if we completed data reading, freeing resources imediatly, 
                // because this ReadResponse will never be called again
                // and any open file will left open forever
                if (completed)
                {
                    this.FreeResources();
                }

                return completed;
            }
        }

        /// <inheritdoc/>
        protected override bool Open(CefRequest request, out bool handleRequest, CefCallback callback)
        {
            handleRequest = false;
            return false;
        }

        /// <inheritdoc/>
        protected override bool Skip(long bytesToSkip, out long bytesSkipped, CefResourceSkipCallback callback)
        {
            bytesSkipped = 0;
            return true;
        }

        /// <inheritdoc/>
        protected override bool Read(IntPtr dataOut, int bytesToRead, out int bytesRead, CefResourceReadCallback callback)
        {
            bytesRead = -1;
            return false;
        }

        /// <inheritdoc/>
        protected override void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        private void FreeBuffer()
        {
            if (_rentedBuffer != null)
            {
                ArrayPool<byte>.Shared.Return(_rentedBuffer);
                _rentedBuffer = null;
            }
        }

        private void FreeResources()
        {
            FreeBuffer();
            _dataStream?.Dispose();
            _dataStream = null;
        }
    }
}
