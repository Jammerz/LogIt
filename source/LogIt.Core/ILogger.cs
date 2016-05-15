﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILogger
    {
        string Identifier { get; set; }

        string Name { get; set; }

        bool HasReader { get; set; }

        bool HasSearcher { get; set; }

        bool HasWriter { get; set; }

        LoggingOptions Options { get; set; }

        ILogReader GetReader();

        ILogSearcher GetSearcher();

        ILogWriter GetWriter();        
    }
}