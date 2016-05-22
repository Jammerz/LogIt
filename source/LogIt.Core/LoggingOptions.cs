using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public class LoggingOptions
    {
        public LogLevel MinimumLogLevel { get; set; }

        public LoggingDetail DetailLevel { get; set; }
        
        public LoggingOptions()
        {
            ApplyDefaultOptions();
        }

        protected virtual void ApplyDefaultOptions()
        {
            MinimumLogLevel = LogLevel.Debug;
            DetailLevel = LoggingDetail.Standard;
        }
    }
}
