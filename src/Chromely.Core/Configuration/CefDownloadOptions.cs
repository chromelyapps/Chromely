namespace Chromely.Core.Configuration
{
    public class CefDownloadOptions : ICefDownloadOptions
    {
        public bool AutoDownloadWhenMissing { get; set; }
        public bool DownloadSilently { get; set; }

        public CefDownloadOptions()
        {
            AutoDownloadWhenMissing = true;
            DownloadSilently = false;
        }

        public CefDownloadOptions(bool autoDownload, bool silentDownload)
        {
            AutoDownloadWhenMissing = autoDownload;
            DownloadSilently = silentDownload;
        }
    }
}