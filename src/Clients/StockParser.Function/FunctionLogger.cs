using Microsoft.Extensions.Logging;
using StockParser.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.Function
{
    public class FunctionLogger : ICustomLogger
    {
        private readonly ILogger _log;

        public FunctionLogger(ILogger log)
        {
            _log = log;
        }
        public void LogDebug(string text)
        {
            _log.LogDebug(text);
        }

        public void LogError(string text)
        {
            _log.LogError(text);
        }

        public void LogError(Exception ex, string text)
        {
            _log.LogError(ex,text);
        }

        public void LogInformation(string text)
        {
            _log.LogInformation(text);
        }

        public void LogTrace(string text)
        {
            _log.LogTrace(text);
        }
    }
}
