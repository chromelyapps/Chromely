// Copyright � 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Loader;

/// <summary>
/// Writes CEF binaries download notification messages to application Logger set.
/// </summary>
public class LoggerCefDownloadNotification : ICefDownloadNotification
{
    private const string InfoHeader = "********************** CEF Binaries Download **********************";
    private const string InfoTail = "*******************************************************************";

    /// <inheritdoc/>
    public virtual void OnDownloadStarted()
    {
        Logger.Instance.Log.LogInformation(Environment.NewLine);
        Logger.Instance.Log.LogInformation(InfoHeader);
        Logger.Instance.Log.LogInformation("CEF binaries download started.");
        Logger.Instance.Log.LogInformation("Note that depending on your network, this might take up to 4 minutes to complete.");
        Logger.Instance.Log.LogInformation("Please wait...");
        Logger.Instance.Log.LogInformation(InfoTail);
        Logger.Instance.Log.LogInformation(Environment.NewLine);
    }

    /// <inheritdoc/>
    public virtual void OnDownloadCompleted(TimeSpan duration)
    {
        Logger.Instance.Log.LogInformation(Environment.NewLine);
        Logger.Instance.Log.LogInformation(InfoHeader);
        Logger.Instance.Log.LogInformation("CEF binaries download completed successfully.");
        Logger.Instance.Log.LogInformation($"Time elapsed: {duration}.");
        Logger.Instance.Log.LogInformation(InfoTail);
        Logger.Instance.Log.LogInformation(Environment.NewLine);
    }

    /// <inheritdoc/>
    public virtual void OnDownloadFailed(Exception exception)
    {
        Logger.Instance.Log.LogInformation(Environment.NewLine);
        Logger.Instance.Log.LogInformation(InfoHeader);
        Logger.Instance.Log.LogInformation("CEF binaries download completed with error.");
        Logger.Instance.Log.LogInformation("Error message: " + exception.Message);
        Logger.Instance.Log.LogInformation(InfoTail);
        Logger.Instance.Log.LogInformation(Environment.NewLine);
    }

    /// <inheritdoc/>
    public virtual void Cleanup()
    {
    }
}