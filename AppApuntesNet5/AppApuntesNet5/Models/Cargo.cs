using System; 
using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel.DataAnnotations; 
using System.Collections.Generic; 


namespace AppApuntesNet5.Models 
{ 
	[Table("cargo")] 
	public class Cargo 
	{ 
		[Column("visible")] 
		public bool visible  { get; set; } 

		[Column("activo")] 
		public bool activo  { get; set; } 

		[Key] 
		[Column("id")] 
		public int id  { get; set; } 

		[Column("descripcion")] 
		public string descripcion  { get; set; } 

		[Column("authority")] 
		public string authority  { get; set; } 

		public virtual List<UsuarioCargo> listaUsuarioCargo  { get; set; } = new List<UsuarioCargo>(); 

		
		// [NotMapped]  // Atributo no mapeado en la base de datos 
		// public string Estado { get; set; } 

	} 
 
}
