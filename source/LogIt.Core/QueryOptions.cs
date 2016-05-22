using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIt.Core
{
    public class QueryOptions
    {
        public List<LoggingType> LogTypes { get; set; }

        public LoggingDetail SearchDetailLevel { get; set; }

        public DateTime? LoggedFrom { get; set; }

        public DateTime? LoggedTo { get; set; }

        public int? Page { get; set; }

        public int? PageSize { get; set; }
    }
}
