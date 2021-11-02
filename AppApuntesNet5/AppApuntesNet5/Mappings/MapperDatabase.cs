using AppApuntesNet5.Dto;
using AppApuntesNet5.Models;


namespace AppApuntesNet5.Mappings
{
    public static partial class Mapper
    {

        public static ApuntesCategoria ToDatabaseObject(this Dto.ApuntesCategoriaDTO dto)
        {
            if (dto == null) return null;

            ApuntesCategoria model = new ApuntesCategoria()
            {
                Id = dto.Id,
                Titulo = dto.Titulo
            };

            return model;
        }

        public static ApuntesTema ToDatabaseObject(this Dto.ApuntesTemaDTO dto)
        {
            if (dto == null) return null;

            ApuntesTema model = new ApuntesTema()
            {
                Id = dto.Id,
                Titulo = dto.Titulo,
                ApuntesCategoria = dto.ApuntesCategoria?.ToDatabaseObject()
            };

            foreach (ApuntesDetalleTemaDTO c in dto.ListaApuntesDetalleTema)
            {
                c.ApuntesTema = null;  // Impedir loop infinito 
                model.ListaApuntesDetalleTema.Add(c.ToDatabaseObject());
            }

            return model;
        }


        public static ApuntesDetalleTema ToDatabaseObject(this Dto.ApuntesDetalleTemaDTO dto)
        {
            if (dto == null) return null;

            return new ApuntesDetalleTema()
            {
                Id = dto.Id,
                Contenido = dto.Contenido,
                RutaFoto = dto.RutaFoto,
                ApuntesTema = dto.ApuntesTema?.ToDatabaseObject(),
                Titulo = dto.Titulo
            };
        }

    }
}
