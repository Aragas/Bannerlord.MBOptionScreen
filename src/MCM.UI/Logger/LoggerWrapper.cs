using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;

namespace MCM.UI.Logger
{
    public class LoggerWrapper<T> : ILogger<T>, ILogger
    {
        private readonly ILogger _loggerImplementation;

        public LoggerWrapper(IServiceProvider serviceProvider) => _loggerImplementation = serviceProvider.GetRequiredService<ILogger<T>>();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _loggerImplementation.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _loggerImplementation.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _loggerImplementation.BeginScope(state);
        }
    }
    public class LoggerWrapper : ILogger
    {
        private readonly ILogger _loggerImplementation;

        public LoggerWrapper(IServiceProvider serviceProvider) => _loggerImplementation = serviceProvider.GetRequiredService<ILogger>();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _loggerImplementation.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _loggerImplementation.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _loggerImplementation.BeginScope(state);
        }
    }
}