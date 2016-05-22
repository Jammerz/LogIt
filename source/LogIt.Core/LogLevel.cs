using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public enum LogLevel
    {
        Undefined = 0,
        Critical = 100,
        Error = 90,
        Warning = 70,
        Info = 50,
        Debug = 20,
        Trace = 10
    }
}
