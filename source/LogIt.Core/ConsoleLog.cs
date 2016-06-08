using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public class ConsoleLog : ILogWriter
    {
        private const string NAME = "Console Log Writer";

        private LogConfig _Config;
        public LogConfig Config {
            get { return Config; }
            set {
                // TODO: ConsoleLog.Config : Lock config when setting.
                // TODO: ConsoleLog.Config : Reload or reset anyrhing that requires it when Config is set.
                _Config = value;
            }
        }

        private Guid _Identifier;
        public Guid Identifier {
            get { return _Identifier; }
            private set { _Identifier = value; }
        }

        private string _Name;
        public string Name {
            get { return _Name; }
            private set { _Name = value; }
        }

        public ConsoleLog()
            : this(config: null)
        {

        }
        public ConsoleLog(LogConfig config)
        {
            Identifier = Guid.NewGuid();
            Name = NAME;
            Config = config ?? new LogConfig();
        }

        public void Dispose()
        {
            // TODO: ConsoleLog.Dispose : At some point we may need to add functionality to attach to or create a new console window. If so dispose will finally have a purpose here.
        }

        public IEnumerable<LogBase> Write(List<LogBase> logs)
        {
            if (logs == null)
                return null;

            for (int logIdx = 0; logIdx < logs.Count; logIdx++)
                Write(logs[logIdx]);

            return logs;
        }

        public LogBase Write(LogBase log)
        {
            if (log != null)
                WriteToConsole(log.Message);

            return log;
        }

        public void WriteToConsole(string message)
        {
            if (message == null)    
                return;             // No .WriteLine() convention for you!

            // Thought too much about this... Decided not to offer option to Write or WriteLine...
            // ... We are writing logs not outputting to a stream, so Write does not really make sense
            Console.WriteLine(message);
        }
    }
}
