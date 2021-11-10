using System.Collections.Generic;

namespace Mappings
{
    public class ApuntesCategoriaDTO  // NUEVO 2-11-2021
    {
        public ApuntesCategoriaDTO()
        {
            ListaApuntesTema = new List<ApuntesTemaDTO>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public List<ApuntesTemaDTO> ListaApuntesTema { get; set; }
    }
}

