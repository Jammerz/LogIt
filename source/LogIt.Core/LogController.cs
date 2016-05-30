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
        private Dictionary<Guid, ILogWriter> _Writers;
        public Dictionary<Guid, ILogWriter> Writers {
            get { return _Writers; }
            private set { _Writers = value ?? new Dictionary<Guid, ILogWriter>(); }
        }
        private readonly object WritersLock = new object();

        private ControllerConfig _Config;
        public ControllerConfig Config {
            get { return _Config; }
            set { _Config = value ?? new ControllerConfig(); }
        }
        private readonly object ConfigLock = new object();

        private bool _ControllerLoggingEnabled;
        public bool ControllerLoggingEnabled {
            get { return _ControllerLoggingEnabled; }
            set {
                ToggleControllerLogging(value);
            }
        }
        private readonly object ControllerLoggingEnabledLock = new object();

        public event LogWriterError OnWriterError;
        public delegate LogWriterErrorArgs LogWriterError(object sender, LogWriterErrorArgs args);

        private LogController _ControllerDebugLog;

        public LogController()
        {
            Writers = new Dictionary<Guid, ILogWriter>();
            Config = new ControllerConfig();
        }
        public LogController(ControllerConfig config)
            : this()
        {
            Config = config;
        }

        public virtual void Register(ILogWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            lock (WritersLock)
            {
                if (!Writers.ContainsKey(writer.Identifier))
                    Writers.Add(writer.Identifier, writer);
                else
                    throw new ArgumentException("A writer with a matching identifier is already registered with this controller.", nameof(writer));
            }            
        }

        public virtual void DeList(Guid identifer)
        {
            lock (WritersLock)
            {
                ILogWriter writer = null;
                if (Writers.TryGetValue(identifer, out writer))
                {
                    Writers.Remove(identifer);

                    writer.Dispose();

                    writer = null;
                }
            }
        }

        public virtual void Write(List<LogBase> logs)
        {
            if (logs?.Count == 0)
                return;

            // Grab current list of log writers
            List<ILogWriter> writers = Writers.Values.ToList();

            if (writers?.Count > 0) 
            {
                // Use while and for loop. Loop like this so we can catch and...
                // ... handle log writer exception without the overhead of...
                // ... initialising a new try catch each itteration... 
                // ... (unless we have some very dodgy writers).
                int writeCount = 0;
                while (writeCount < writers.Count)
                {
                    try
                    {
                        for (; writeCount < writers.Count; writeCount++)
                        {
                            if (logs.Count == 1)
                                writers[writeCount].Write(logs[0]);
                            else
                                writers[writeCount].Write(logs);
                        }
                    }
                    catch (Exception exc)
                    {
                        HandleWriterException(writers[writeCount], String.Format("Error writing to log. Message: {0}", exc.Message), exc);
                        writeCount++;
                    }
                }
            }
        }

        public virtual void Write(LogBase log)
        {
            if (log == null)
                return;

            // Use list override of method to avoid repeating ourselves. 
            // The list initialisation for each individual log call may be...
            // ... considered too much overhead. I'm stubornly trying to... 
            // ... keep my code pretty over keeping it pratical. 
            Write(new List<LogBase>(1) { log });
        }

        public Log Write(string message, LogLevel logType, NameValueCollection logDetail = null)
        {
            throw new NotImplementedException();
        }

        public Log WriteException(Exception exc, LogLevel logType, NameValueCollection logDetail = null)
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

            // Initialise or dispose controller log depending on value
            if (loggingEnabled)    
                InitControllerLog();
            else
                DisposeControllerLog();
        }

        private void InitControllerLog()
        {
            WriteToControllerLog("Reinitialising controller log..."); // this will only be seen if an existing controller log exists

            if (_ControllerDebugLog != null)
                DisposeControllerLog(); // Dispose and reset if ControllerLog already exists

            lock (ControllerLoggingEnabledLock)
            {
                // TODO: LogController.InitControllerLog : Customise config so it is suitable for controller logging.
                ControllerConfig config = new ControllerConfig();
                _ControllerDebugLog = new LogController(config);

                // TODO: LogController.InitControllerLog : Add standard output log once developed
                
                // TODO: LogController.InitControllerLog : Add method to add user specified ILogWriters to controller logging (specified in controller config maybe?)

                _ControllerLoggingEnabled = true;
            }

            WriteToControllerLog("Controller log initialised.");
        }

        private void DisposeControllerLog()
        {
            WriteToControllerLog("Disposing current controller log...");

            lock (ControllerLoggingEnabledLock)
            {
                // Set here to prevent further logging attempts during disposal
                _ControllerLoggingEnabled = false;

                if (_ControllerDebugLog == null)
                    return;

                _ControllerDebugLog.Dispose();
                _ControllerDebugLog = null;                
            }
        }

        private void WriteToControllerLog(string message, NameValueCollection logDetail = null)
        {
            if (ControllerLoggingEnabled)   // Do not use lock here as Debug call *could* potentially take some time and we do not want to lock the property for any amount of time
                _ControllerDebugLog?.Debug(message, logDetail); // Use ternary as best thread safe call to ControllerLog without using lock
            else
                return;
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
