using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public class LogController : IDisposable
    {
        private List<ILogWriter> _Writers;
        public List<ILogWriter> Writers {
            get { return _Writers; }
            private set { _Writers = value ?? new List<ILogWriter>(); }
        }

        private ControllerConfig _Config;
        public ControllerConfig Config {
            get { return _Config; } 
            set { _Config = value ?? new ControllerConfig(); }
        }

        private bool _ControllerLoggingEnabled;
        public bool ControllerLoggingEnabled {
            get { return _ControllerLoggingEnabled; }
            set {
                ToggleControllerLogging(value);
            }
        }

        public event LogWriterError OnWriterError;
        public delegate LogWriterErrorArgs LogWriterError(object sender, LogWriterErrorArgs args);

        private LogController _ControllerDebugLog;

        public LogController()
        {
            Writers = new List<ILogWriter>();
            Config = new ControllerConfig();
        }
        public LogController(ControllerConfig config)
            : this()
        {
            Config = config;
        }

        public virtual ILogWriter Register(ILogWriter logger)
        {
            throw new NotImplementedException();
        }

        public virtual void DeList(Guid identifer)
        {
            throw new NotImplementedException();
        }

        public virtual LogBase Write(List<LogBase> logs)
        {
            throw new NotImplementedException();
        }

        public virtual LogBase Write(LogBase log)
        {
            throw new NotImplementedException();
        }

        public LogBase Write(string message, LogLevel logType, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public LogBase WriteException(Exception exc, LogLevel logType, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public Log Critical(string message, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public Log CriticalException(Exception exc, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public Log Error(string message, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public Log ErrorException(Exception exc, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public Log Warning(string message, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public Log WarningException(Exception exc, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public Log Info(string message, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public Log Debug(string message, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public Log Trace(string message, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void HandleWriterException(ILogWriter writer, string message, Exception exc)
        {
            throw new NotImplementedException();
        }

        private void ToggleControllerLogging(bool loggingEnabled)
        {
            throw new NotImplementedException();
        }

        private void InitControllerLog()
        {
            throw new NotImplementedException();
        }

        private void DisposeControllerLog()
        {
            throw new NotImplementedException();
        }
    }

    public class LogWriterErrorArgs : EventArgs
    {
        private ILogWriter _Writer;
        public ILogWriter Writer {
            get { return _Writer; }
            private set { _Writer = value; }
        }

        private string _Message;
        public string Message {
            get { return _Message; }
            private set { _Message = value ?? String.Empty; }
        }

        private Exception _Exception;
        public Exception Exception {
            get { return _Exception;  }
            set { _Exception = value; }
        }

        public LogWriterErrorArgs()
            : base()
        {
            Writer = null;
            Message = null;
            Exception = null;
        }
        public LogWriterErrorArgs(ILogWriter writer, Exception exc)
            : this(writer, exc != null ? exc.Message : String.Empty, exc)
        {

        }
        public LogWriterErrorArgs(ILogWriter writer, string message)
            : this(writer, message, null)
        {

        }
        public LogWriterErrorArgs(ILogWriter writer, string message, Exception exc)
            : base()
        {
            Writer = writer;
            Message = message;
            Exception = exc;
        }
    }
}
