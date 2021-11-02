using System.Collections.Generic;

namespace AppApuntesNet5.Dto
{
    public class ApuntesTemaDTO  // NUEVO 2-11-2021
    {
        public ApuntesTemaDTO()
        {
            ListaApuntesDetalleTema = new List<ApuntesDetalleTemaDTO>();
        }
        
        public int Id { get; set; }
        public string Titulo { get; set; }
        public ApuntesCategoriaDTO ApuntesCategoria { get; set; }
        public List<ApuntesDetalleTemaDTO> ListaApuntesDetalleTema { get; set; }
    }
}

