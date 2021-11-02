using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace AppApuntesNet5.Models
{
    [Table("cargo")]
    public class Cargo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("visible")]
        public bool Visible { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("authority")]
        public string Authority { get; set; }

        public virtual List<UsuarioCargo> ListaUsuarioCargo { get; set; } = new List<UsuarioCargo>();

    }

}
