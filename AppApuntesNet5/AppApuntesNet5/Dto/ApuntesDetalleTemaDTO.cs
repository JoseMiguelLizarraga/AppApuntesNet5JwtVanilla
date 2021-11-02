
namespace AppApuntesNet5.Dto
{
    public class ApuntesDetalleTemaDTO  // NUEVO 2-11-2021
    {
        public ApuntesDetalleTemaDTO()
        {
        }
        public int Id { get; set; }
        public string Contenido { get; set; }
        public string RutaFoto { get; set; }
        public ApuntesTemaDTO ApuntesTema { get; set; }
        public string Titulo { get; set; }
    }
}

