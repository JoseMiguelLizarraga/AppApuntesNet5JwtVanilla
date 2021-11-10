using DataAccess.Models;
using System.Linq;

namespace Mappings
{
    public static partial class Mapper
    {
        public static ApuntesCategoriaDTO ToDTO(this ApuntesCategorium model)
        {
            if (model == null) return null;

            ApuntesCategoriaDTO dto = new ApuntesCategoriaDTO()
            {
                Id = model.Id,
                Titulo = model.Titulo,
                Logo = model.Logo,
                TipoLogo = model.TipoLogo
            };

            return dto;
        }

        public static ApuntesTemaDTO ToDTO(this ApuntesTema model)
        {
            if (model == null) return null;

            ApuntesTemaDTO dto = new ApuntesTemaDTO()
            {
                Id = model.Id,
                Titulo = model.Titulo,
                ApuntesCategoria = model.Categoria?.ToDTO()
            };

            if (model.ApuntesDetalleTemas != null)  // Esto es necesario, ya que este metodo es llamado para llenar la lista de detalles del tema
            {
                foreach (ApuntesDetalleTema c in model.ApuntesDetalleTemas)
                {
                    c.Tema = null;  // Impedir loop infinito 
                    dto.ListaApuntesDetalleTema.Add(c.ToDTO());
                }
            }

            return dto;
        }

        public static ApuntesDetalleTemaDTO ToDTO(this ApuntesDetalleTema model)
        {
            if (model == null) return null;

            if (model.Tema != null)
                model.Tema.ApuntesDetalleTemas = null;  // Esto es necesario, ya que el metodo ApuntesTemaDTO deja en null el campo ApuntesTema

            return new ApuntesDetalleTemaDTO()
            {
                Id = model.Id,
                Contenido = model.Contenido,
                RutaFoto = model.RutaFoto,
                ApuntesTema = model.Tema.ToDTO(),
                Titulo = model.Titulo
            };
        }
    }
}


