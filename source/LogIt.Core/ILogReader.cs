using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILogReader : IDisposable
    {
        long Position { get; }

        bool EndOfLogReached { get; }

        ILog GetNext();

        IEnumerable<ILog> GetNext(int count);

        IEnumerable<ILog> GetAll();

        void ResetPosition();
    }
}
