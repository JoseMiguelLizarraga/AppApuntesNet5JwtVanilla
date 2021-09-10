using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Dto
{
    public class Select2DTO
    {
        [JsonProperty("Total")]
        public int Total { get; set; }

        [JsonProperty("Results")]
        public object Results { get; set; }
    }
}
