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
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            WriteToControllerLog(String.Format("Processing error from writer '{0}'...", writer.Identifier));

            // TODO: LogController.HandleWriterException : Retrieve throwWriterExceptions value from Config
            bool throwWriterExceptions = false;
            
            if (throwWriterExceptions)
            {
                WriteToControllerLog(String.Concat("... ", nameof(throwWriterExceptions), " was true. Throwing writer exception"));
                throw exc != null ? exc : new Exception(message ?? String.Empty);
            }
            else
            {
                WriteToControllerLog(String.Concat("... ", nameof(throwWriterExceptions), " was false. Raising writer error event."));
                OnWriterError?.Invoke(this, new LogWriterErrorArgs(writer, message, exc));
            }
        }

        private void ToggleControllerLogging(bool loggingEnabled)
        {
            WriteToControllerLog(String.Format("Toggling controller logging from {0} to {1}.", ControllerLoggingEnabled, loggingEnabled));

            if (_ControllerLoggingEnabled == loggingEnabled)
                return; // No need to bother if current equals value

            _ControllerLoggingEnabled = loggingEnabled; // Set here to prevent further logging attempts during disposal (if value == false)

            if (loggingEnabled)  // Initialise or dispose controller log depending on value
                InitControllerLog();
            else
                DisposeControllerLog();
        }

        private void InitControllerLog()
        {
            WriteToControllerLog("Reinitialising controller log..."); // this will only be seen if an existing controller log exists

            if (_ControllerDebugLog != null)
                DisposeControllerLog(); // Dispose and reset if ControllerLog already exists

            // TODO: LogController.InitControllerLog : Customise config so it is suitable for controller logging.
            ControllerConfig config = new ControllerConfig();
            _ControllerDebugLog = new LogController(config);

            // TODO: LogController.InitControllerLog : Add standard output log once developed


            // TODO: LogController.InitControllerLog : Add method to add user specified ILogWriters to controller logging

            WriteToControllerLog("Controller log initialised.");
        }

        private void DisposeControllerLog()
        {
            WriteToControllerLog("Disposing current controller log...");

            if (_ControllerDebugLog == null)
                return;

            _ControllerDebugLog.Dispose();
            _ControllerDebugLog = null;
        }

        private void WriteToControllerLog(string message, NameValueCollection logDetail = null)
        {
            if (!ControllerLoggingEnabled)
                return;

            _ControllerDebugLog.Debug(message, logDetail);
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
            get { return _Exception; }
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
