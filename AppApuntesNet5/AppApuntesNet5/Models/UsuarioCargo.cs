using System; 
using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel.DataAnnotations; 


namespace AppApuntesNet5.Models 
{ 
	[Table("usuario_cargo")] 
	public class UsuarioCargo 
	{ 
		[Key] 
		[Column("id")] 
		public int id  { get; set; } 

		//[JsonIgnore]  // Impedir problema generado al convertir a json la clase Cargo 
		public virtual Cargo cargo  { get; set; } 

		[ForeignKey("cargo")] 
		[Column("cargo_id")] 
		public int cargoId  { get; set; } 

		//[JsonIgnore]  // Impedir problema generado al convertir a json la clase Usuario 
		public virtual Usuario usuario  { get; set; } 

		[ForeignKey("usuario")] 
		[Column("usuario_id")] 
		public int usuarioId  { get; set; } 

		
		// [NotMapped]  // Atributo no mapeado en la base de datos 
		// public string Estado { get; set; } 

	} 
 
}
