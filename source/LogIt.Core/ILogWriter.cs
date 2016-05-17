﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILogWriter
    {
        Log Write(ILoggable log);

        IEnumerable<Log> Write(IEnumerable<ILoggable> logs);
    }
}
