using AppApuntesNet5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Services
{

	public interface ITemaService
	{
		//private ApplicationDbContext db;

		public List<ApuntesTema> listar();
		public ApuntesTema buscarPorId(int id);
		public IDictionary<string, object> llenarDataTableApuntesTema(ApuntesTema apuntesTema, int inicio, int registrosPorPagina);
		public IDictionary<string, object> llenarSelect2(String clase, String busqueda, int registrosPorPagina, int numeroPagina);
		public ApuntesTema guardar(ApuntesTema apuntesTema);
		public ApuntesTema actualizar(ApuntesTema apuntesTema);
		public void eliminar(int id);
		public void procesarDatosExcel(List<ApuntesTema> elementosInsertados, List<ApuntesTema> elementosActualizados);
		public void validarApuntesTema(ApuntesTema apuntesTema);
	}
}
