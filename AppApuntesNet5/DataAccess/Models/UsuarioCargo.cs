using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class UsuarioCargo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int CargoId { get; set; }

        public virtual Cargo Cargo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
