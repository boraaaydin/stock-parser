using Microsoft.Extensions.Logging;
using StockParser.Data;
using StockParser.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.ConsoleClient
{
    class ConsoleLogger:ICustomLogger
    {
        private ILogger _logger;

        public ConsoleLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ConsoleLogger");
        }

        public void LogTrace(string text)
        {
            _logger.LogTrace(text);
        }

        public void LogDebug(string text)
        {
            _logger.LogDebug(text);
        }

        public void LogError(string text)
        {
            _logger.LogError(text);
        }

        public void LogInformation(string text)
        {
            _logger.LogInformation(text);
        }

        public void LogError(Exception ex, string text)
        {
            _logger.LogError(text);
        }
    }
}
