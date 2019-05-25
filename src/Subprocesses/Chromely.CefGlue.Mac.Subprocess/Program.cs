// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Mac.Subprocess
{
    class Program
    {
        public static int Main(string[] args)
        {
            return CefGlue.Subprocess.Subprocess.Execute(args);
        }
    }
}
