// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubprocessArguments.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Chromely.CefGlue.Subprocess
{
    internal static class SubprocessArguments
    {
        public const string HostProcessIdArgument = "--host-process-id";
        public const string CustomSchemeArgument = "--custom-scheme";
        public const string CustomCmdlineArgument = "--custom-cmdline-args";
        public const string FocusedNodeChangedEnabledArgument = "--focused-node-enabled";
        public const string ArgumentType = "--type";
        public const string ExitIfParentProcessClosed = "--cefgluepexitsub";
        public const char Separator = ';';
        public const char ChildSeparator = '|';

        public static bool HasArgument(this IEnumerable<string> args, string arg)
        {
            return args.Any(a => a.StartsWith(arg));
        }

        public static string GetArgumentValue(this IEnumerable<string> args, string argumentName)
        {
            var arg = args?.FirstOrDefault(a => a.StartsWith(argumentName));
            return arg?.Split('=').Last();
        }
    }
}
