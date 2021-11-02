using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace AppApuntesNet5.Models
{
    [Table("apuntes_detalle_tema")]
    public class ApuntesDetalleTema
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("ruta_foto")]
        public string RutaFoto { get; set; }

        [Column("contenido")]
        public String Contenido { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; }

        public virtual ApuntesTema ApuntesTema { get; set; }

        [ForeignKey("apuntes_tema")]
        [Column("tema_id")]
        public int ApuntesTemaId { get; set; }

    }

}
