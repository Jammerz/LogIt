﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILogConnection : IDisposable
    {
        bool IsOpen();

        void Open();

        void Close();
    }
}
