// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Win.Subprocess_x64
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return Subprocess.Subprocess.Execute(args);
        }
    }
}