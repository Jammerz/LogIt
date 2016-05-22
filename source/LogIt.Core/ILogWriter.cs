using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILogWriter : ILogConnection
    {
        Guid Identifier { get; }

        string Name { get; }

        LogConfig Config { get; set; }

        Log Write(LogBase log);

        IEnumerable<Log> Write(List<LogBase> logs);
    }
}
