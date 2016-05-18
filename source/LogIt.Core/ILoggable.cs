using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILoggable
    {
        /// <summary>
        /// Consolidate and return the ILoggable instance as a LogBase instance.
        /// </summary>
        /// <returns>Representation of instance as LogBase.</returns>
        LogBase GetLog();
    }
}
