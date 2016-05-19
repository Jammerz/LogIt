using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILogReader : ILogConnection
    {
        long ReaderPosition { get; }

        bool EndOfLogReached { get; }

        Log GetNext();

        IEnumerable<Log> GetNext(int count);

        IEnumerable<Log> GetAll();

        void ResetPosition();

        long SeekPosition(long position);
    }
}
