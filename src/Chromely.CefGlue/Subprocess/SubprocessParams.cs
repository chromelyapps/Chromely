// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubprocessParams.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Chromely.CefGlue.Subprocess
{
    using System.Linq;

    internal class SubprocessParams
    {
        public List<string> CustomSchemes { get; set; }
        public List<Tuple<string, string, bool>> CommandLineArgs { get; set; }

        public static SubprocessParams Parse(IEnumerable<string> subprocessArgs)
        {
            var processParams = new SubprocessParams();

            if (subprocessArgs == null || !subprocessArgs.Any())
            {
                return processParams;
            }

            var customSchemesValue = subprocessArgs.GetArgumentValue(SubprocessArguments.CustomSchemeArgument);
            var customCmdlinesValue = subprocessArgs.GetArgumentValue(SubprocessArguments.CustomCmdlineArgument);

            if (string.IsNullOrWhiteSpace(customSchemesValue))
            {
                return processParams;
            }

            if (string.IsNullOrWhiteSpace(customCmdlinesValue))
            {
                return processParams;
            }

            processParams.CustomSchemes = customSchemesValue.Split(SubprocessArguments.Separator).ToList();
            var customCmdlinesParts = customCmdlinesValue.Split(SubprocessArguments.Separator).ToList();

            processParams.CommandLineArgs = new List<Tuple<string, string, bool>>();

            foreach (var part in customCmdlinesParts)
            {
                var cmdline = part.Split(SubprocessArguments.ChildSeparator).ToList();
                if (cmdline.Count < 3)
                {
                    continue;
                }

                var item1 = cmdline[0];
                var item2 = cmdline[1];
                var item3 = cmdline[2].Equals("T");

                processParams.CommandLineArgs.Add(new Tuple<string, string, bool>(item1, item2, item3));
            }

            return processParams;
        }
    }
}
