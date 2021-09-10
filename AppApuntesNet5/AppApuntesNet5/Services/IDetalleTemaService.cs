using AppApuntesNet5.Dto;
using AppApuntesNet5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Services
{
	public interface IDetalleTemaService
	{
		public List<ApuntesDetalleTema> Listar();
		public ApuntesDetalleTema BuscarPorId(int id);
		public DataTableDTO LlenarDataTableApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema, int inicio, int registrosPorPagina);
		public (ApuntesDetalleTema, string) Guardar(ApuntesDetalleTema apuntesDetalleTema);
		public (ApuntesDetalleTema, string) Actualizar(ApuntesDetalleTema apuntesDetalleTema);
		public bool Eliminar(int id, out string mensajeError);
		public bool ValidarApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema, out string mensajeError);
	}
}
