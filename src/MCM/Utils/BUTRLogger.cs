using BUTR.DependencyInjection.Logger;

using System.Diagnostics;

namespace MCM.Utils
{
    internal class BUTRLogger : IBUTRLogger
    {
        public void LogMessage(LogLevel logLevel, string message, params object[] args)
        {
            switch (logLevel)
            {
                //case LogLevel.Trace:
                //case LogLevel.Debug:
                //case LogLevel.None:
                default:
                    Trace.WriteLine(message);
                    break;
                case LogLevel.Information:
                    Trace.TraceInformation(message);
                    break;
                case LogLevel.Warning:
                    Trace.TraceWarning(message);
                    break;
                case LogLevel.Error:
                    Trace.TraceError(message);
                    break;
                case LogLevel.Critical:
                    Trace.TraceError(message);
                    break;
            }
        }
    }
    internal class BUTRLogger<T> : IBUTRLogger<T>
    {
        public void LogMessage(LogLevel logLevel, string message, params object[] args)
        {
            switch (logLevel)
            {
                //case LogLevel.Trace:
                //case LogLevel.Debug:
                //case LogLevel.None:
                default:
                    Trace.WriteLine(message);
                    break;
                case LogLevel.Information:
                    Trace.TraceInformation(message);
                    break;
                case LogLevel.Warning:
                    Trace.TraceWarning(message);
                    break;
                case LogLevel.Error:
                    Trace.TraceError(message);
                    break;
                case LogLevel.Critical:
                    Trace.TraceError(message);
                    break;
            }
        }
    }
}
