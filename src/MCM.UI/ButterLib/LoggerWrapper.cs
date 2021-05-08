using BUTR.DependencyInjection.Logger;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;

namespace MCM.UI.ButterLib
{
    public class LoggerWrapper : IBUTRLogger
    {
        private readonly ILogger _logger;

        public LoggerWrapper(ILogger logger)
        {
            _logger = logger;
        }

        public void LogMessage(BUTR.DependencyInjection.Logger.LogLevel logLevel, string message, params object[] args)
        {
            _logger.Log((Microsoft.Extensions.Logging.LogLevel) logLevel, default, new FormattedLogValues(message, args), null, (state, _) => state.ToString());
        }
    }

    public class LoggerWrapper<T> : IBUTRLogger<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerWrapper(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogMessage(BUTR.DependencyInjection.Logger.LogLevel logLevel, string message, params object[] args)
        {
            _logger.Log((Microsoft.Extensions.Logging.LogLevel) logLevel, default, new FormattedLogValues(message, args), null, (state, _) => state.ToString());
        }
    }
}