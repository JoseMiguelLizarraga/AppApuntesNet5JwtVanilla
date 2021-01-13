using System; 
using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel.DataAnnotations; 
using System.Collections.Generic; 


namespace AppApuntesNet5.Models 
{ 
	[Table("apuntes_tema")] 
	public class ApuntesTema 
	{ 
		//[JsonIgnore]  // Impedir problema generado al convertir a json la clase ApuntesCategoria 
		public virtual ApuntesCategoria apuntesCategoria  { get; set; } 

		[ForeignKey("apuntes_categoria")] 
		[Column("categoria_id")] 
		public int apuntesCategoriaId  { get; set; } 

		[Key] 
		[Column("id")] 
		public int id  { get; set; } 

		[Column("titulo")] 
		public string titulo  { get; set; } 

		public virtual List<ApuntesDetalleTema> listaApuntesDetalleTema  { get; set; } = new List<ApuntesDetalleTema>(); 

		
		// [NotMapped]  // Atributo no mapeado en la base de datos 
		// public string Estado { get; set; } 

	} 
 
}
