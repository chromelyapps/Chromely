// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

/// <summary>
/// Sets of notication callback methods in CEF binaries download cycle.
/// </summary>
public interface ICefDownloadNotification
{
    /// <summary>
    /// Method that will run when download starts.
    /// </summary>
    void OnDownloadStarted();

    /// <summary>
    /// Method that will run when download completes.
    /// </summary>
    /// <param name="duration"></param>
    void OnDownloadCompleted(TimeSpan duration);

    /// <summary>
    /// Method that will run when download fails.
    /// </summary>
    /// <param name="exception">The exception on failure.</param>
    void OnDownloadFailed(Exception exception);

    /// <summary>
    /// Clean up methods - example html notification generates temporary files. 
    /// The files should be deleted here.
    /// </summary>
    void Cleanup();
}
