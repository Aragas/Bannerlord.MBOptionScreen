using System;
using System.Diagnostics;

namespace MCM.Logger
{
    internal class DefaultMCMLogger<T> : IMCMLogger<T>
    {
        public void LogTrace(string message, params object[] args) => Trace.WriteLine(string.Format(message, args));
        public void LogInformation(string message, params object[] args) => Trace.TraceInformation(message, args);
        public void LogWarning(string message, params object[] args) => Trace.TraceWarning(message, args);
        public void LogError(Exception exception, string message, params object[] args) => Trace.TraceError(message, args);
    }

    internal class DefaultMCMLogger : IMCMLogger
    {
        public void LogTrace(string message, params object[] args) => Trace.WriteLine(string.Format(message, args));
        public void LogInformation(string message, params object[] args) => Trace.TraceInformation(message, args);
        public void LogWarning(string message, params object[] args) => Trace.TraceWarning(message, args);
        public void LogError(Exception exception, string message, params object[] args) => Trace.TraceError(message, args);
    }
}