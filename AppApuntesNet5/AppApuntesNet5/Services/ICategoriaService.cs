using AppApuntesNet5.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApuntesNet5.Services
{
    public interface ICategoriaService
    {
        public Select2DTO LlenarSelect2(String busqueda, int registrosPorPagina, int numeroPagina);
    }
}
