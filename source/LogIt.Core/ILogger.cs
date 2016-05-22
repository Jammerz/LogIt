using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILogger : IDisposable
    {
        Guid Identifier { get; }

        string Description { get; }

        LogConfig Options { get; set; }

        ILogReader GetReader();

        ILogQuery GetSearcher();

        ILogWriter GetWriter();        
    }
}
