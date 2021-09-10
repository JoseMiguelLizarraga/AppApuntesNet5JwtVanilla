using AppApuntesNet5.Dto;
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

		public List<ApuntesTema> Listar();
		public ApuntesTema BuscarPorId(int id);
		public DataTableDTO LlenarDataTableApuntesTema(ApuntesTema apuntesTema, int inicio, int registrosPorPagina);
		public Select2DTO LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria);
		public (ApuntesTema, string) Guardar(ApuntesTema apuntesTema);
		public (ApuntesTema, string) Actualizar(ApuntesTema apuntesTema);
		public bool Eliminar(int id, out string mensajeError);
		public void ProcesarDatosExcel(List<ApuntesTema> elementosInsertados, List<ApuntesTema> elementosActualizados);
		public bool ValidarApuntesTema(ApuntesTema apuntesTema, out string mensajeError);
	}
}
