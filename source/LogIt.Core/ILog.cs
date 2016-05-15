using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public interface ILog
    {
        DateTime LogTime { get; set; }

        LoggingType LogType { get; set; }

        string Message { get; set; }

        NameValueCollection Detail { get; set; }
    }
}
