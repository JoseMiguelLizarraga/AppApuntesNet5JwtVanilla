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
        public DataTableDTO LlenarDataTableApuntesCategoria(ApuntesCategoria apuntesCategoria, int inicio, int registrosPorPagina);

        //public Task<Select2DTO> LlenarSelect2(String busqueda, int registrosPorPagina, int numeroPagina);

        public Select2DTO LlenarSelect2(String busqueda, int registrosPorPagina, int numeroPagina);

        public Task<(ApuntesCategoria, string)> Guardar(ApuntesCategoria objeto);
    }
}
