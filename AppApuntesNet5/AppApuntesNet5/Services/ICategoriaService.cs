using AppApuntesNet5.Dto;
using AppApuntesNet5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Services
{
    public interface ICategoriaService
    {
        public ApuntesCategoria BuscarPorId(int id);
        public DataTableDTO LlenarDataTableApuntesCategoria(ApuntesCategoria apuntesCategoria, int inicio, int registrosPorPagina);
        public Select2DTO LlenarSelect2(String busqueda, int registrosPorPagina, int numeroPagina);
        public Task<(ApuntesCategoria, string)> Guardar(ApuntesCategoria objeto);
        public Task<(ApuntesCategoria, string)> Actualizar(ApuntesCategoria apuntesCategoria);
        public Task<(bool, string)> Eliminar(int id);
    }
}
