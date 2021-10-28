using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace AppApuntesNet5.Models
{
    [Table("usuario_cargo")]
    public class UsuarioCargo
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        public virtual Cargo cargo { get; set; }

        [ForeignKey("cargo")]
        [Column("cargo_id")]
        public int cargoId { get; set; }

        public virtual Usuario usuario { get; set; }

        [ForeignKey("usuario")]
        [Column("usuario_id")]
        public int usuarioId { get; set; }

    }

}
