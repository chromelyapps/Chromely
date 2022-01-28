// Copyright � 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Loader;

/// <summary>
/// Writes CEF binaries download notification messages to Console output.
/// </summary>
public class ConsoleCefDownloadNotification : ICefDownloadNotification
{
    private const string InfoHeader = "********************** CEF Binaries Download **********************";
    private const string InfoTail = "*******************************************************************";

    /// <inheritdoc/>
    public virtual void OnDownloadStarted()
    {
        Console.WriteLine();
        Console.WriteLine(InfoHeader);
        Console.WriteLine("CEF binaries download started.");
        Console.WriteLine("Note that depending on your network, this might take up to 4 minutes to complete.");
        Console.WriteLine("Please wait...");
        Console.WriteLine(InfoTail);
        Console.WriteLine();
    }

    /// <inheritdoc/>
    public virtual void OnDownloadCompleted(TimeSpan duration)
    {
        Console.WriteLine();
        Console.WriteLine(InfoHeader);
        Console.WriteLine("CEF binaries download completed successfully.");
        Console.WriteLine($"Time elapsed: {duration}.");
        Console.WriteLine(InfoTail);
        Console.WriteLine();
    }

    /// <inheritdoc/>
    public virtual void OnDownloadFailed(Exception exception)
    {
        Console.WriteLine();
        Console.WriteLine(InfoHeader);
        Console.WriteLine("CEF binaries download completed with error.");
        Console.WriteLine("Error message: " + exception.Message);
        Console.WriteLine(InfoTail);
        Console.WriteLine();
    }

    /// <inheritdoc/>
    public virtual void Cleanup()
    {
    }
}