using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using Mappings;
using Util;

namespace Services
{
    public interface ICategoriaService
    {
        public ApuntesCategorium BuscarPorId(int id);
        public DataTableDTO LlenarDataTableApuntesCategoria(ApuntesCategorium apuntesCategoria, int inicio, int registrosPorPagina);
        public Select2DTO LlenarSelect2(String busqueda, int registrosPorPagina, int numeroPagina);
        public Task<(ApuntesCategorium, ExcepcionCapturada)> Guardar(ApuntesCategorium objeto);
        public Task<(ApuntesCategorium, ExcepcionCapturada)> Actualizar(ApuntesCategorium apuntesCategoria);
        public Task<(bool, ExcepcionCapturada)> Eliminar(int id);
        public Task<(ApuntesCategorium, ExcepcionCapturada)> GuardarLogo(ApuntesCategorium objeto);
    }
}
