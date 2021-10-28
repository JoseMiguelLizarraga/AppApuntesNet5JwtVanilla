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
        public Task<List<ApuntesTema>> Listar();
        public Task<ApuntesTema> BuscarPorId(int id);
        public Task<DataTableDTO> LlenarDataTableApuntesTema(ApuntesTema apuntesTema, int inicio, int registrosPorPagina);
        public Task<Select2DTO> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina, int idApuntesCategoria);
        public Task<(ApuntesTema, string)> Guardar(ApuntesTema apuntesTema);
        public Task<(ApuntesTema, string)> Actualizar(ApuntesTema apuntesTema);
        public Task<(bool, string)> Eliminar(int id);
        public bool ValidarApuntesTema(ApuntesTema apuntesTema, out string mensajeError);
    }
}
