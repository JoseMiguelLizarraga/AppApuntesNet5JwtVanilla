using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace AppApuntesNet5.Models
{
    [Table("apuntes_tema")]
    public class ApuntesTema
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; }

        public virtual ApuntesCategoria ApuntesCategoria { get; set; }

        [ForeignKey("apuntes_categoria")]
        [Column("categoria_id")]
        public int ApuntesCategoriaId { get; set; }

        public virtual List<ApuntesDetalleTema> ListaApuntesDetalleTema { get; set; } = new List<ApuntesDetalleTema>();

    }

}
