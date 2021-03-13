//using Microsoft.Extensions.Logging;
//using StockParser.Domain;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace StockParser.Web.Helpers
//{
//    public class LogHelper : ICustomLogger
//    {
//        private ILogger<T> _logger;

//        public LogHelper(ILogger<T> logger)
//        {
//            _logger = logger;
//        }
//        public void LogDebug(string text)
//        {
//            _logger.LogDebug(text);
//        }

//        public void LogError(string text)
//        {
//            _logger.LogError(text);
//        }

//        public void LogError(Exception ex, string text)
//        {
//            _logger.LogDebug(ex.Message,text);
//        }

//        public void LogInformation(string text)
//        {
//            _logger.LogInformation(text);
//        }

//        public void LogTrace(string text)
//        {
//            _logger.LogTrace(text);
//        }
//    }
//}
