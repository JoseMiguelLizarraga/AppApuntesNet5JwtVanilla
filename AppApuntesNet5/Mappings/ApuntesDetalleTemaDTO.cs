
namespace Mappings
{
    public class ApuntesDetalleTemaDTO
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

