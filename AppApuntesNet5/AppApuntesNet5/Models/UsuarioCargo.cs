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
        public int Id { get; set; }

        public virtual Cargo Cargo { get; set; }

        [ForeignKey("cargo")]
        [Column("cargo_id")]
        public int CargoId { get; set; }

        public virtual Usuario Usuario { get; set; }

        [ForeignKey("usuario")]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

    }

}
