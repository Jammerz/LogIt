using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public class QueryOptions
    {
        public List<LogLevel> LogTypes { get; set; }

        public LogQuality ReturnedLogQuality { get; set; }

        public DateTime? LoggedFrom { get; set; }

        public DateTime? LoggedTo { get; set; }

        public int? Page { get; set; }

        public int? PageSize { get; set; }
    }
}
