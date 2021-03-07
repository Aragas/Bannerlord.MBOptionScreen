using MCM.Logger;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;

namespace MCM.UI.Logger
{
    public class MCMLogger<T> : IMCMLogger<T>
    {
        private readonly ILogger _logger;

        public MCMLogger(IServiceProvider serviceProvider) => _logger = serviceProvider.GetRequiredService<ILogger<T>>();

        public void LogTrace(string message, params object[] args) => _logger.LogTrace(message, args);
        public void LogInformation(string message, params object[] args) => _logger.LogInformation(message, args);
        public void LogWarning(string message, params object[] args) => _logger.LogWarning(message, args);
        public void LogError(Exception exception, string message, params object[] args) => _logger.LogError(exception, message, args);
    }
    public class MCMLogger : IMCMLogger
    {
        private readonly ILogger _logger;

        public MCMLogger(IServiceProvider serviceProvider) => _logger = serviceProvider.GetRequiredService<ILogger>();

        public void LogTrace(string message, params object[] args) => _logger.LogTrace(message, args);
        public void LogInformation(string message, params object[] args) => _logger.LogInformation(message, args);
        public void LogWarning(string message, params object[] args) => _logger.LogWarning(message, args);
        public void LogError(Exception exception, string message, params object[] args) => _logger.LogError(exception, message, args);
    }
}