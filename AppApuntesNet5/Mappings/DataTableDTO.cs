using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mappings
{
    public class DataTableDTO
    {
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public object Data { get; set; }
    }
}
