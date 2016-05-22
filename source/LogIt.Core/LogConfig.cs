using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public class LogConfig
    {
        public LogLevel MinimumLogLevel { get; set; }

        public LogQuality WriteQuality { get; set; }
        
        public LogConfig()
        {
            ApplyDefaultOptions();
        }

        protected virtual void ApplyDefaultOptions()
        {
            MinimumLogLevel = LogLevel.Debug;
            DetailLevel = LogQuality.Standard;
        }
    }
}
