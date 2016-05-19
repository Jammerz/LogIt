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

        LoggingOptions Options { get; set; }

        ILogReader GetReader();

        ILogSearcher GetSearcher();

        ILogWriter GetWriter();        
    }
}
