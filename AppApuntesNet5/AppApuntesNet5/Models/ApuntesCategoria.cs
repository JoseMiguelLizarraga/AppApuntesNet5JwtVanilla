using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace AppApuntesNet5.Models
{
    [Table("apuntes_categoria")]
    public class ApuntesCategoria
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; }

        public virtual List<ApuntesTema> ListaApuntesTema { get; set; } = new List<ApuntesTema>();

    }

}
