using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Dto
{
    public class DataTableDTO
    {
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public object Data { get; set; }
    }
}
