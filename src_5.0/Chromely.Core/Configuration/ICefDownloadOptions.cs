namespace Chromely.Core.Configuration
{
    public interface ICefDownloadOptions
    {
        bool AutoDownloadWhenMissing { get; set; }
        bool DownloadSilently { get; set; }
    }
}