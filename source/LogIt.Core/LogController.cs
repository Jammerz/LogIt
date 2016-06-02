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

            WriteToControllerLog("{0}: Adding  writer '{1}' to controller...", nameof(Register), writer.Identifier);

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
            WriteToControllerLog("{0}: Removing writer '{1}' from controller...", nameof(DeList), identifer);

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

            // Grab current list of log writers, holding the lock for as short a time as possible
            List<ILogWriter> writers = null;
            lock (WritersLock)
            {
                writers = Writers.Values.ToList();
            }
            
            if (writers.Count > 0) 
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
                        HandleWriterException(writers[writeCount], 
                            String.Format("Error writing to '{0}'. Error: {1}", writers[writeCount].Name ?? writers[writeCount].Identifier.ToString(), exc.Message), 
                            exc);

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
            Log log = new Log(message, logType, logDetail);

            Write(log);

            return log;
        }

        public Log WriteFormat(LogLevel logType, string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));

            return Write(String.Format(format, args), logType, null);
        }

        public Log WriteException(Exception exc, LogLevel logType, NameValueCollection logDetail = null)
        {
            // TODO: LogController.WriteException : Format message 'better'. Maybe use a publicly settable formatter?
            string message = exc != null && exc.Message != null ? exc.Message : String.Empty;

            return Write(message, logType, logDetail);
        }

        public Log Critical(string message, NameValueCollection logDetail = null)
        {
            return Write(message, LogLevel.Critical, logDetail);
        }

        public Log CriticalFormat(string format, params object[] args)
        {
            return WriteFormat(LogLevel.Critical, format, args);
        }

        public Log CriticalException(Exception exc, NameValueCollection logDetail = null)
        {
            return WriteException(exc, LogLevel.Critical, logDetail);
        }

        public Log Error(string message, NameValueCollection logDetail = null)
        {
            return Write(message, LogLevel.Error, logDetail);
        }

        public Log ErrorFormat(string format, params object[] args)
        {
            return WriteFormat(LogLevel.Error, format, args);
        }

        public Log ErrorException(Exception exc, NameValueCollection logDetail = null)
        {
            return WriteException(exc, LogLevel.Error, logDetail);
        }

        public Log Warning(string message, NameValueCollection logDetail = null)
        {
            return Write(message, LogLevel.Warning, logDetail);
        }

        public Log WarningFormat(string format, params object[] args)
        {
            return WriteFormat(LogLevel.Warning, format, args);
        }

        public Log WarningException(Exception exc, NameValueCollection logDetail = null)
        {
            return WriteException(exc, LogLevel.Warning, logDetail);
        }

        public Log Info(string message, NameValueCollection logDetail = null)
        {
            return Write(message, LogLevel.Info, logDetail);
        }

        public Log InfoFormat(string format, params object[] args)
        {
            return WriteFormat(LogLevel.Info, format, args);
        }

        public Log Debug(string message, NameValueCollection logDetail = null)
        {
            return Write(message, LogLevel.Debug, logDetail);
        }

        public Log DebugFormat(string format, params object[] args)
        {
            return WriteFormat(LogLevel.Debug, format, args);
        }

        public Log Trace(string message, NameValueCollection logDetail = null)
        {
            return Write(message, LogLevel.Trace, logDetail);
        }

        public Log TraceFormat(string format, params object[] args)
        {
            return WriteFormat(LogLevel.Trace, format, args);
        }

        public void Dispose()
        {
            WriteToControllerLog("{0}: Disposing LocController...", nameof(Dispose));

            lock (WritersLock)
            {
                List<ILogWriter> writers = Writers.Values.ToList();

                if (writers?.Count > 0)
                {
                    for (int remainingDisposals = writers.Count; remainingDisposals > 0; remainingDisposals--)
                    {
                        ILogWriter writer = writers[remainingDisposals - 1];

                        try
                        {
                            // Cannot use Delist as this also uses the WritersLock object
                            writer.Dispose();

                            Writers.Remove(writer.Identifier);

                            writer = null;

                            writers.RemoveAt(remainingDisposals - 1);
                        }
                        catch (Exception exc)
                        {
                            HandleWriterException(writer,
                                String.Format("Error disposing '{0}'. Error: {1}", writer.Name ?? writer.Identifier.ToString(), exc.Message),
                                exc);
                        }
                    }
                }

                // TODO: LogController.Dispose : Complete disposal of instace.
            }            
        }

        private void HandleWriterException(ILogWriter writer, string message, Exception exc)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            WriteToControllerLog("{0}: Processing error from writer '{1}'...", nameof(HandleWriterException), writer.Identifier);

            // TODO: LogController.HandleWriterException : Retrieve throwWriterExceptions value from Config
            bool throwWriterExceptions = false;
            
            if (throwWriterExceptions)
            {
                WriteToControllerLog("{0}: ... {1} was true. Throwing writer exception", nameof(HandleWriterException), nameof(throwWriterExceptions));
                throw exc != null ? exc : new Exception(message ?? String.Empty);
            }
            else
            {
                WriteToControllerLog("{0}: ... {1} was false. Raising writer error event.", nameof(HandleWriterException), nameof(throwWriterExceptions));
                OnWriterError?.Invoke(this, new LogWriterErrorArgs(writer, message, exc));
            }
        }

        private void ToggleControllerLogging(bool loggingEnabled)
        {
            WriteToControllerLog("{0}: Switching controller logging from {1} to {2}.", nameof(ToggleControllerLogging), ControllerLoggingEnabled, loggingEnabled);
            
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
            WriteToControllerLog("{0}: Reinitialising controller log...", nameof(InitControllerLog)); // this will only be seen if an existing controller log exists

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

            WriteToControllerLog("{0}: ... Controller log initialised.", nameof(InitControllerLog));
        }

        private void DisposeControllerLog()
        {
            WriteToControllerLog("{0}: Disposing current controller log...", nameof(DisposeControllerLog));

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

        private void WriteToControllerLog(Log log)
        {
            if (ControllerLoggingEnabled)   // Do not use lock here as Debug call *could* potentially take some time and we do not want to lock the property for any amount of time
                _ControllerDebugLog?.Write(log); // Use ternary as best thread safe call to ControllerLog without using lock
            else
                return;
        }

        private void WriteToControllerLog(string message)
        {
            WriteToControllerLog(new Log(message, LogLevel.Debug));
        }

        private void WriteToControllerLog(string format, params object[] args)
        {
            WriteToControllerLog(String.Format(format, args));
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
