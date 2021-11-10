using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class ApuntesDetalleTema
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public string RutaFoto { get; set; }
        public int TemaId { get; set; }

        public virtual ApuntesTema Tema { get; set; }
    }
}
