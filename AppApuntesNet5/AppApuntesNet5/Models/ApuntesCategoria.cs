using System; 
using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel.DataAnnotations; 
using System.Collections.Generic; 


namespace AppApuntesNet5.Models 
{ 
	[Table("apuntes_categoria")] 
	public class ApuntesCategoria 
	{ 
		[Key] 
		[Column("id")] 
		public int id  { get; set; } 

		[Column("titulo")] 
		public string titulo  { get; set; } 

		public virtual List<ApuntesTema> listaApuntesTema  { get; set; } = new List<ApuntesTema>(); 

		
		// [NotMapped]  // Atributo no mapeado en la base de datos 
		// public string Estado { get; set; } 

	} 
 
}
