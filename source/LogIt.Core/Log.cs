using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public class LogBase : ILoggable
    {
        public DateTime LogTime { get; set; }

        public LoggingType LogType { get; set; }

        public string Message { get; set; }

        public NameValueCollection Detail { get; set; }

        /// <summary>
        /// Base implementation of an ILoggable instance. 
        /// For use in high volume logging scenarios where the overhead of 
        /// variable assingments and initialisations could become costly.
        /// For scenarios less performance sensitive use or inherit Log.
        /// </summary>
        public LogBase()
        {
            // Nothing... 
            // No assumptions...
            // No variable assignments...
            // No collection initialisations... 
            // NOTHING!
            // 
            // **************************************************************************
            // **************************************************************************
            // The point of LogBase is to provide the lightest possible implmentation...
            // ... of a Log instace for use in high volume logging scenarios where the... 
            // ... overhead of assingments and initialisations could become costly.
            // **************************************************************************
            // **************************************************************************
        }
        
        public LogBase GetLog()
        {
            return this;
        }
    }

    public class Log : LogBase
    {
        private static StringComparer DEFAULT_KEY_COMPARISON = StringComparer.InvariantCultureIgnoreCase;

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
            : base()
        {
            LogTime = logTime;
            Message = message ?? String.Empty;
            LogType = logType;
            Detail = new NameValueCollection(DEFAULT_KEY_COMPARISON) { logDetail ?? new NameValueCollection() };
        }
    }
}
