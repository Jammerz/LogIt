using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILogSearcher : IDisposable
    {
        IEnumerable<Log> Search(LogSearchOptions queryOptions);
    }
}
