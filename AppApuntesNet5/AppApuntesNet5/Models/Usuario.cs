using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace AppApuntesNet5.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("rut")]
        public string Rut { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("apellido_materno")]
        public string apellidoMaterno { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("apellido_paterno")]
        public string ApellidoPaterno { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [Column("telefono")]
        public string Telefono { get; set; }

        [Column("largo_password")]
        public int LargoPassword { get; set; }

        [Column("clave_pdt")]
        public string ClavePdt { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("visible")]
        public bool Visible { get; set; }

        public virtual List<UsuarioCargo> ListaUsuarioCargo { get; set; } = new List<UsuarioCargo>();

    }

}
