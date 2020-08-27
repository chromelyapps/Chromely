using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.BZip2
{
    /// <summary>
    /// An example class to demonstrate compression and decompression of BZip2 streams.
    /// </summary>
    public static class BZip2
    {
        /// <summary>
        /// Decompress the <paramref name="inStream">input</paramref> writing 
        /// uncompressed data to the <paramref name="outStream">output stream</paramref>
        /// </summary>
        /// <param name="inStream">The readable stream containing data to decompress.</param>
        /// <param name="outStream">The output stream to receive the decompressed data.</param>
        /// <param name="isStreamOwner">Both streams are closed on completion if true.</param>
        /// <param name="progressPercent">Callback action to provide progress feedback.</param>
        public static void Decompress(Stream inStream, Stream outStream, bool isStreamOwner, Action<int> progressPercent = null)
        {
            if (inStream == null || outStream == null)
            {
                throw new Exception("Null Stream");
            }

            try
            {
                using (BZip2InputStream bzipInput = new BZip2InputStream(inStream))
                {
                    bzipInput.IsStreamOwner = isStreamOwner;
                    Core.StreamUtils.Copy(bzipInput, outStream, new byte[4096], progressBytes =>
                    {
                        var percent = (int)(progressBytes * 100.0 / inStream.Length);
                        progressPercent?.Invoke(percent);
                    });
                }
            }
            finally
            {
                if (isStreamOwner)
                {
                    // inStream is closed by the BZip2InputStream if stream owner
                    outStream.Dispose();
                }
            }
        }
    }
}