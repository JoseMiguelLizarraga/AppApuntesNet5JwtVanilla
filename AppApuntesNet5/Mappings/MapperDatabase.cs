using DataAccess.Models;

namespace Mappings
{
    public static partial class Mapper
    {

        public static ApuntesCategorium ToDatabaseObject(this ApuntesCategoriaDTO dto)
        {
            if (dto == null) return null;

            ApuntesCategorium model = new ApuntesCategorium()
            {
                Id = dto.Id,
                Titulo = dto.Titulo,
                Logo = dto.Logo,
                TipoLogo = dto.TipoLogo
            };

            return model;
        }

        public static ApuntesTema ToDatabaseObject(this ApuntesTemaDTO dto)
        {
            if (dto == null) return null;

            ApuntesTema model = new ApuntesTema()
            {
                Id = dto.Id,
                Titulo = dto.Titulo,
                Categoria = dto.ApuntesCategoria?.ToDatabaseObject()
            };

            foreach (ApuntesDetalleTemaDTO c in dto.ListaApuntesDetalleTema)
            {
                c.ApuntesTema = null;  // Impedir loop infinito 
                model.ApuntesDetalleTemas.Add(c.ToDatabaseObject());
            }

            return model;
        }


        public static ApuntesDetalleTema ToDatabaseObject(this ApuntesDetalleTemaDTO dto)
        {
            if (dto == null) return null;

            return new ApuntesDetalleTema()
            {
                Id = dto.Id,
                Contenido = dto.Contenido,
                RutaFoto = dto.RutaFoto,
                Tema = dto.ApuntesTema?.ToDatabaseObject(),
                Titulo = dto.Titulo
            };
        }

    }
}
