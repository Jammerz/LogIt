using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILogWriter : IDisposable
    {
        Guid Identifier { get; }

        string Name { get; }

        LogConfig Config { get; set; }

        LogBase Write(LogBase log);

        IEnumerable<LogBase> Write(List<LogBase> logs);
    }
}
