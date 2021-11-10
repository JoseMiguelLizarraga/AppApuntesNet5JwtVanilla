using System.Collections.Generic;

namespace Mappings
{
    public class ApuntesCategoriaDTO
    {
        public ApuntesCategoriaDTO()
        {
            ListaApuntesTema = new List<ApuntesTemaDTO>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public byte[] Logo { get; set; }
        public string TipoLogo { get; set; }
        public List<ApuntesTemaDTO> ListaApuntesTema { get; set; }
    }
}

