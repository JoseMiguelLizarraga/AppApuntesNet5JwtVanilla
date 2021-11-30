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
        public RespuestaService<ApuntesCategorium> BuscarPorId(int id);
        public RespuestaService<DataTableDTO> LlenarDataTableApuntesCategoria(ApuntesCategorium apuntesCategoria, int inicio, int registrosPorPagina);
        public RespuestaService<Select2DTO> LlenarSelect2(String busqueda, int registrosPorPagina, int numeroPagina);
        public Task<RespuestaService<ApuntesCategorium>> Guardar(ApuntesCategorium objeto);
        public Task<RespuestaService<ApuntesCategorium>> Actualizar(ApuntesCategorium apuntesCategoria);
        public Task<(bool, ExcepcionCapturada)> Eliminar(int id);
        public Task<RespuestaService<ApuntesCategorium>> GuardarLogo(ApuntesCategorium objeto);
    }
}
