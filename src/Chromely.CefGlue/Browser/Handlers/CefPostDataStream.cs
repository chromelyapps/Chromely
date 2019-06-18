// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoXFrameOptionsHttpSchemeHandler.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Browser.Handlers
{
    /// <summary>
    /// Provides a view of a sequence of bytes in CefPostDataElement[] array.
    /// </summary>
    public class CefPostDataStream : Stream
    {
        readonly int _length;       // Number of bytes within the memory stream

        int _currentElementIndex;
        int _currentElementPosition;
        CefPostDataElement[] _postDataElements;
        byte[] _currentElementBytes;

        private int _position;     // read/write head.
        private bool _isOpen;      // Is this stream open or closed?

        /// <summary>
        /// Initializes a new instance of the Chromely.CefGlue.Browser.Handlers.CefPostDataStream class.
        /// </summary>
        /// <param name="postDataElements">The source of bytes stream</param>        
        public CefPostDataStream(CefPostDataElement[] postDataElements)
        {
            _postDataElements = postDataElements ?? throw new ArgumentNullException(nameof(postDataElements));
            _length = _postDataElements.DefaultIfEmpty().Sum(x => (int)x.BytesCount);

            _currentElementIndex = 0;
            _currentElementPosition = 0;
            _isOpen = true;
        }

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => false;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        private void EnsureNotClosed()
        {
            if (!_isOpen)
                throw new InvalidOperationException("Stream is closed");
        }

        private void EnsureWriteable()
        {
            if (!CanWrite)
                throw new NotSupportedException("Cef stream is not writable");
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _isOpen = false;
                    _postDataElements = null;
                }
            }
            finally
            {
                // Call base.Close() to cleanup async IO resources
                base.Dispose(disposing);
            }
        }

        /// <inheritdoc/>
        public override void Flush()
        {
        }

        /// <inheritdoc/>
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            try
            {
                Flush();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        /// <inheritdoc/>
        public override long Length => throw new NotSupportedException();

        /// <inheritdoc/>
        public override long Position
        {
            get
            {
                EnsureNotClosed();
                return _position;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                throw new InvalidOperationException("CefPostDataStream is not seeckable");
                //EnsureNotClosed();

                //if (value > MemStreamMaxLength)
                //    throw new ArgumentOutOfRangeException(nameof(value), SR.ArgumentOutOfRange_StreamLength);
                //_position = _origin + (int)value;
            }
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - offset < count)
                throw new ArgumentException("Invalid offset lenght");

            EnsureNotClosed();

            int totalBytesToCopy = _length - _position;
            if (totalBytesToCopy > count)
                totalBytesToCopy = count;
            if (totalBytesToCopy <= 0)
                return 0;

            Debug.Assert(_position + totalBytesToCopy >= 0, "_position + n >= 0");  // len is less than 2^31 -1.
            int bytesCopied = 0;

            while (bytesCopied < totalBytesToCopy)
            {
                var postElement = _postDataElements[_currentElementIndex];
                int bytesLeftInCurrentElement = (int)postElement.BytesCount - _currentElementPosition;
                int bytesToCopyFromCurrentElement = Math.Min(bytesLeftInCurrentElement, totalBytesToCopy - bytesCopied);

                if (_currentElementBytes == null)
                    _currentElementBytes = postElement.GetBytes();

                if (bytesToCopyFromCurrentElement <= 8)
                {
                    var byteCount = bytesToCopyFromCurrentElement;
                    while (--byteCount >= 0)
                        buffer[offset + byteCount] = _currentElementBytes[_currentElementPosition + byteCount];
                }
                else
                    Buffer.BlockCopy(_currentElementBytes, _currentElementPosition, buffer, offset, bytesToCopyFromCurrentElement);

                _currentElementPosition += bytesToCopyFromCurrentElement;
                offset += bytesToCopyFromCurrentElement;
                bytesCopied += bytesToCopyFromCurrentElement;

                if (_currentElementPosition == postElement.BytesCount) //if we done with current element
                {
                    _currentElementBytes = null;
                    _currentElementPosition = 0;
                    _currentElementIndex++;
                }
            }
            _position += totalBytesToCopy;
            return totalBytesToCopy;
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        //TODO: make more efficient implementation ReadByte
    }
}
