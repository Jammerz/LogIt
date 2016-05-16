using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public class Log
    {
        private static StringComparer DEFAULT_KEY_COMPARISON = StringComparer.InvariantCultureIgnoreCase;

        DateTime LogTime { get; set; }

        LoggingType LogType { get; set; }

        string Message { get; set; }

        NameValueCollection Detail { get; set; }

        public Log()
            : this(String.Empty, LoggingType.Debug)
        {

        }
        public Log(string message, LoggingType logType)
            : this(message, logType, DateTime.Now)
        {

        }
        public Log(string message, LoggingType logType, DateTime logTime)
            : this(message, logType, logTime, null)
        {

        }
        public Log(string message, LoggingType logType, DateTime logTime, NameValueCollection logDetail)
        {
            LogTime = logTime;
            Message = message ?? String.Empty;
            LogType = logType;
            Detail = new NameValueCollection(DEFAULT_KEY_COMPARISON) { logDetail ?? new NameValueCollection() };
        }
    }
}
