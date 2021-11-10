using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class ApuntesCategorium
    {
        public ApuntesCategorium()
        {
            ApuntesTemas = new HashSet<ApuntesTema>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }

        public virtual ICollection<ApuntesTema> ApuntesTemas { get; set; }
    }
}
