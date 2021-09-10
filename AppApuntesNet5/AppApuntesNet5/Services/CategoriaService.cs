using AppApuntesNet5.Dto;
using AppApuntesNet5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Services
{
    public class CategoriaService : ICategoriaService
    {
		private ApplicationDbContext db;

		public CategoriaService(ApplicationDbContext context)
		{
			this.db = context;
		}

		public Select2DTO LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
		{
			object dataSalida = null;
			IQueryable<ApuntesCategoria> consulta = (from a in db.ApuntesCategoria select a);

			if (!string.IsNullOrEmpty(busqueda))
				consulta = consulta.Where(a => a.titulo.Contains(busqueda));

			int cantidadRegistros = consulta.Count();
			consulta.Skip((numeroPagina - 1) * registrosPorPagina).Take(registrosPorPagina);
			dataSalida = consulta.ToList().Select(a => new { id = a.id, text = a.titulo }).ToList();

			return new Select2DTO() { Total = cantidadRegistros, Results = dataSalida};
		}
	}
}
