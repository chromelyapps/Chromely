// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestLogger.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.Core.Tests
{
    public class TestLogger : IChromelyLogger
    {
        public string Message;
        
        public void Info(string message)
        {
            Message = message;
        }

        public void Verbose(string message)
        {
            Message = message;
        }

        public void Debug(string message)
        {
            Message = message;
        }

        public void Warn(string message)
        {
            Message = message;
        }

        public void Critial(string message)
        {
            Message = message;
        }

        public void Fatal(string message)
        {
            Message = message;
        }

        public void Error(string message)
        {
            Message = message;
        }

        public void Error(Exception exception, string message = null)
        {
            Message = message;
        }
    }
}
