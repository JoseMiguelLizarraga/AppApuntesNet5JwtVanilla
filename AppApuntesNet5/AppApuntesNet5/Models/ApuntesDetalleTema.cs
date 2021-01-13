using System; 
using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel.DataAnnotations; 


namespace AppApuntesNet5.Models 
{ 
	[Table("apuntes_detalle_tema")] 
	public class ApuntesDetalleTema 
	{ 
		[Column("ruta_foto")] 
		public string rutaFoto  { get; set; } 

		[Key] 
		[Column("id")] 
		public int id  { get; set; } 

		[Column("contenido")] 
		public String contenido  { get; set; } 

		[Column("titulo")] 
		public string titulo  { get; set; } 

		//[JsonIgnore]  // Impedir problema generado al convertir a json la clase ApuntesTema 
		public virtual ApuntesTema apuntesTema  { get; set; } 

		[ForeignKey("apuntes_tema")] 
		[Column("tema_id")] 
		public int apuntesTemaId  { get; set; } 

		
		// [NotMapped]  // Atributo no mapeado en la base de datos 
		// public string Estado { get; set; } 

	} 
 
}
