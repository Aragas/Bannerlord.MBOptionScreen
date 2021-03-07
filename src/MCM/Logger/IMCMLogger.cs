using System;

namespace MCM.Logger
{
    public interface IMCMLogger<T> : IMCMLogger { }

    public interface IMCMLogger
    {
        void LogTrace(string message, params object[] args);
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
    }
}