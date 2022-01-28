// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// The console message event args.
/// </summary>
public class ConsoleMessageEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMessageEventArgs"/> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="source">
    /// The source.
    /// </param>
    /// <param name="line">
    /// The line.
    /// </param>
    public ConsoleMessageEventArgs(string message, string source, int line)
    {
        Message = message;
        Source = source;
        Line = line;
    }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Gets the source.
    /// </summary>
    public string Source { get; private set; }

    /// <summary>
    /// Gets the line.
    /// </summary>
    public int Line { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether handled.
    /// </summary>
    public bool Handled { get; set; }
}