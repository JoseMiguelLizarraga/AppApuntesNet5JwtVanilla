using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Services
{
    public class RespuestaService<T>
    {
        public T Objeto { get; set; }
        public ExcepcionCapturada ExcepcionCapturada { get;set;}
    }
}
