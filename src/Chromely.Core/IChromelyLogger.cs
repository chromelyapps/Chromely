using System;

namespace Chromely.Core
{
    public interface IChromelyLogger
    {
        void Info(string message);
        void Verbose(string message);
        void Debug(string message);
        void Warn(string message);
        void Critial(string message);
        void Fatal(string message);
        void Error(string message);
        void Error(Exception exception, string message = null);
    }
}