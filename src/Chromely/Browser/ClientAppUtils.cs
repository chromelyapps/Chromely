// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

public enum ProcessType
{
    None,
    Browser,
    Renderer,
    Zygot,
    Other,
}

/// <summary>
/// Utility class to determine browser and child processes.
/// </summary>
/// <remarks>
/// CEF3 runs using multiple processes. The main process which handles window creation,
/// painting and network access is called the “browser” process.
/// This is generally the same process as the host application
/// and the majority of the application logic will run in the browser process.
///
/// Reference - https://bitbucket.org/chromiumembedded/cef/wiki/GeneralUsage
/// </remarks>
public static class ClientAppUtils
{
    const string ArgumentType = "--type";
    const string RendererType = "renderer";
    const string ZygoteType = "zygote";

    /// <summary>
    /// Checks if process or subprocess is allowed to execute based on the OS platform.
    /// </summary>
    /// <param name="platform">The OS platorm.</param>
    /// <param name="args">Command line arguments.</param>
    /// <returns>true if allowed, otherwise false.</returns>
    public static bool IsProcessExecutionAllowed(ChromelyPlatform platform, IEnumerable<string> args)
    {
        if (platform != ChromelyPlatform.MacOSX)
        {
            return true;
        }

        return HasArgument(args, ArgumentType);
    }

    /// <summary>
    /// Gets the current process type.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>the process type identified by <see cref="ProcessType"/>.</returns>
    public static ProcessType GetProcessType(IEnumerable<string> args)
    {
        if (args is null || !args.Any())
        {
            return ProcessType.Browser;
        }

        if (HasArgument(args, ArgumentType))
        {
            string? type = GetArgumentValue(args, ArgumentType);
            if (type is not null && !string.IsNullOrWhiteSpace(type) && type.Equals(RendererType, StringComparison.InvariantCultureIgnoreCase))
            {
                return ProcessType.Renderer;
            }

            type = GetArgumentValue(args, ArgumentType);
            if (type is not null && !string.IsNullOrWhiteSpace(type) && type.Equals(ZygoteType, StringComparison.InvariantCultureIgnoreCase))
            {
                return ProcessType.Renderer;
            }

            return ProcessType.Other;
        }

        return ProcessType.Browser;
    }

    /// <summary>
    /// Checks if process has arguments.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <param name="argType">The argument type.</param>
    /// <returns>true if it has argument, otherwise false.</returns>
    private static bool HasArgument(IEnumerable<string> args, string argType)
    {
        return args.Any(a => a.StartsWith(argType, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the argument value
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <param name="argumentName">The argument name.</param>
    /// <returns>the value in string.</returns>
    private static string? GetArgumentValue(this IEnumerable<string> args, string argumentName)
    {
        if (args is not null)
        {
            var arg = args.FirstOrDefault(a => a.StartsWith(argumentName, StringComparison.Ordinal));
            if (arg is not null)
            {
                return arg.Split('=').Last();
            }
        }

        return default;
    }
}