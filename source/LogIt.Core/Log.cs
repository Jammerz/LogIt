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
        /// <summary>
        /// The point in time the log message references. 
        /// Usually the time the log was created.
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// Specifies the type or severity of the log. 
        /// Allows for log read and write filtering
        /// </summary>
        public LoggingType LogType { get; set; }

        /// <summary>
        /// The primary message of the log. 
        /// Usually summarised to allow for quick judgement of log severity.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// For all log detail that does not belong in or would pollute the primary log message.
        /// </summary>
        public NameValueCollection Detail { get; set; }

        /// <summary>
        /// Base implementation of an ILoggable instance. 
        /// For use in high volume logging scenarios where the overhead of 
        /// variable assignments and initialisations could become costly.
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

        /// <summary>
        /// Return LogBase implementation of instance.
        /// </summary>
        /// <returns>LogBase implementation of instance.</returns>
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
            base.GetLog();
        }
    }
}
