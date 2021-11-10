using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using Mappings;

namespace Services
{
    public interface ICategoriaService
    {
        public ApuntesCategorium BuscarPorId(int id);
        public DataTableDTO LlenarDataTableApuntesCategoria(ApuntesCategorium apuntesCategoria, int inicio, int registrosPorPagina);
        public Select2DTO LlenarSelect2(String busqueda, int registrosPorPagina, int numeroPagina);
        public Task<(ApuntesCategorium, string)> Guardar(ApuntesCategorium objeto);
        public Task<(ApuntesCategorium, string)> Actualizar(ApuntesCategorium apuntesCategoria);
        public Task<(bool, string)> Eliminar(int id);
        public Task<(ApuntesCategorium, string)> GuardarLogo(ApuntesCategorium objeto);
    }
}
