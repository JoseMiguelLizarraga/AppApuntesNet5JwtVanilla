using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class ApuntesTema
    {
        public ApuntesTema()
        {
            ApuntesDetalleTemas = new HashSet<ApuntesDetalleTema>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public int CategoriaId { get; set; }

        public virtual ApuntesCategorium Categoria { get; set; }
        public virtual ICollection<ApuntesDetalleTema> ApuntesDetalleTemas { get; set; }
    }
}
