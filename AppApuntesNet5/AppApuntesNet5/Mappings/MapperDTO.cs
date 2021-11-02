using AppApuntesNet5.Dto;
using AppApuntesNet5.Models;

namespace AppApuntesNet5.Mappings
{
    public static partial class Mapper
    {
        public static ApuntesCategoriaDTO ToDTO(this Models.ApuntesCategoria model)
        {
            if (model == null) return null;

            ApuntesCategoriaDTO dto = new ApuntesCategoriaDTO()
            {
                Id = model.Id,
                Titulo = model.Titulo
            };

            return dto;
        }

        public static ApuntesTemaDTO ToDTO(this Models.ApuntesTema model)
        {
            if (model == null) return null;

            ApuntesTemaDTO dto = new ApuntesTemaDTO()
            {
                Id = model.Id,
                Titulo = model.Titulo,
                ApuntesCategoria = model.ApuntesCategoria?.ToDTO()
            };

            foreach (ApuntesDetalleTema c in model.ListaApuntesDetalleTema)
            {
                c.ApuntesTema = null;  // Impedir loop infinito 
                dto.ListaApuntesDetalleTema.Add(c.ToDTO());
            }

            return dto;
        }

        public static ApuntesDetalleTemaDTO ToDTO(this Models.ApuntesDetalleTema model)
        {
            if (model == null) return null;

            return new ApuntesDetalleTemaDTO()
            {
                Id = model.Id,
                Contenido = model.Contenido,
                RutaFoto = model.RutaFoto,
                ApuntesTema = model.ApuntesTema?.ToDTO(),
                Titulo = model.Titulo
            };
        }
    }
}


