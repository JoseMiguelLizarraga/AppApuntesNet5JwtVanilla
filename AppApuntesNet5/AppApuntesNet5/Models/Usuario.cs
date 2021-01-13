using System; 
using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel.DataAnnotations; 
using System.Collections.Generic; 


namespace AppApuntesNet5.Models 
{ 
	[Table("usuario")] 
	public class Usuario 
	{ 
		[Column("username")] 
		public string username  { get; set; } 

		[Column("apellido_materno")] 
		public string apellidoMaterno  { get; set; } 

		[Key] 
		[Column("id")] 
		public int id  { get; set; } 

		[Column("visible")] 
		public bool visible  { get; set; } 

		[Column("apellido_paterno")] 
		public string apellidoPaterno  { get; set; } 

		[Column("activo")] 
		public bool activo  { get; set; } 

		[Column("nombre")] 
		public string nombre  { get; set; } 

		[Column("telefono")] 
		public string telefono  { get; set; } 

		[Column("largo_password")] 
		public int largoPassword  { get; set; } 

		[Column("clave_pdt")] 
		public string clavePdt  { get; set; } 

		[Column("password")] 
		public string password  { get; set; } 

		[Column("rut")] 
		public string rut  { get; set; } 

		public virtual List<UsuarioCargo> listaUsuarioCargo  { get; set; } = new List<UsuarioCargo>(); 

		
		// [NotMapped]  // Atributo no mapeado en la base de datos 
		// public string Estado { get; set; } 

	} 
 
}
