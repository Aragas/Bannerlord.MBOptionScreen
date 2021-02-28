using System;

namespace MCM.Logger
{
    internal class NullMCMLogger<T> : IMCMLogger<T>
    {
        public void LogTrace(string message, params object[] args) { }
        public void LogInformation(string message, params object[] args) { }
        public void LogWarning(string message, params object[] args) { }
        public void LogError(Exception exception, string message, params object[] args) { }
    }

    internal class NullMCMLogger : IMCMLogger
    {
        public void LogTrace(string message, params object[] args) { }
        public void LogInformation(string message, params object[] args) { }
        public void LogWarning(string message, params object[] args) { }
        public void LogError(Exception exception, string message, params object[] args) { }
    }
}