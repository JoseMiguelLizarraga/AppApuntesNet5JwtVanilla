using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace AppApuntesNet5.Models
{
    [Table("apuntes_detalle_tema")]
    public class ApuntesDetalleTema
    {
        [Column("ruta_foto")]
        public string rutaFoto { get; set; }

        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("contenido")]
        public String contenido { get; set; }

        [Column("titulo")]
        public string titulo { get; set; }

        public virtual ApuntesTema apuntesTema { get; set; }

        [ForeignKey("apuntes_tema")]
        [Column("tema_id")]
        public int apuntesTemaId { get; set; }

    }

}
