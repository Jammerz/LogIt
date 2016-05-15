using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public class LoggingOptions
    {
        public LoggingType MinimumLogLevel { get; set; }

        public LoggingDetail DetailLevel { get; set; }
        
        public LoggingOptions()
        {
            ApplyDefaultOptions();
        }

        protected virtual void ApplyDefaultOptions()
        {
            MinimumLogLevel = LoggingType.Debug;
            DetailLevel = LoggingDetail.Standard;
        }
    }
}
