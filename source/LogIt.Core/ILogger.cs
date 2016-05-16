using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILogger : IDisposable
    {
        string Identifier { get; set; }

        string Description { get; set; }

        bool HasReader { get; set; }

        bool HasSearcher { get; set; }

        bool HasWriter { get; set; }

        LoggingOptions Options { get; set; }

        ILogReader GetReader();

        ILogSearcher GetSearcher();

        ILogWriter GetWriter();        
    }
}
