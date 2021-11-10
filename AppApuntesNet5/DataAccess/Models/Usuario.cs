using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            UsuarioCargos = new HashSet<UsuarioCargo>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; }
        public bool Visible { get; set; }

        public virtual ICollection<UsuarioCargo> UsuarioCargos { get; set; }
    }
}
