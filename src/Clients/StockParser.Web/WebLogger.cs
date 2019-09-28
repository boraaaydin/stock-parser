using Microsoft.Extensions.Logging;
using StockParser.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockParser.Web
{
    public class WebLogger : ICustomLogger
    {
        public WebLogger(ILogger<WebLogger> logger)
        {
            Logger = logger;
        }

        public ILogger<WebLogger> Logger { get; }

        public void LogDebug(string text)
        {
            Logger.LogDebug(text);
        }

        public void LogError(string text)
        {
            Logger.LogError(text);
        }

        public void LogError(Exception ex, string text)
        {
            Logger.LogError(ex,text);
        }

        public void LogInformation(string text)
        {
            Logger.LogInformation(text);
        }

        public void LogTrace(string text)
        {
            Logger.LogTrace(text);
        }
    }
}
