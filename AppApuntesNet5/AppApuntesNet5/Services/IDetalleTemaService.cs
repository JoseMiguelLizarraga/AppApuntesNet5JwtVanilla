using AppApuntesNet5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Services
{
	public interface IDetalleTemaService
	{
		public List<ApuntesDetalleTema> listar();
		public ApuntesDetalleTema buscarPorId(int id);
		public IDictionary<string, object> llenarDataTableApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema, int inicio, int registrosPorPagina);
		public IDictionary<string, object> llenarSelect2(String clase, String busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria);
		public ApuntesDetalleTema guardar(ApuntesDetalleTema apuntesDetalleTema);
		public ApuntesDetalleTema actualizar(ApuntesDetalleTema apuntesDetalleTema);
		public void eliminar(int id);
		public void validarApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema);
	}
}
