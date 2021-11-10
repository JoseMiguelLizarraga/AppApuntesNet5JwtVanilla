using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Cargo
    {
        public Cargo()
        {
            UsuarioCargos = new HashSet<UsuarioCargo>();
        }

        public int Id { get; set; }
        public string Authority { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public bool Visible { get; set; }

        public virtual ICollection<UsuarioCargo> UsuarioCargos { get; set; }
    }
}
