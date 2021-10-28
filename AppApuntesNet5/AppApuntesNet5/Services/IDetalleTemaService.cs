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
        public Task<List<ApuntesDetalleTema>> Listar();
        public Task<ApuntesDetalleTema> BuscarPorId(int id);
        public Task<DataTableDTO> LlenarDataTableApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema, int inicio, int registrosPorPagina);
        public Task<(ApuntesDetalleTema, string)> Guardar(ApuntesDetalleTema apuntesDetalleTema);
        public Task<(ApuntesDetalleTema, string)> Actualizar(ApuntesDetalleTema apuntesDetalleTema);
        public Task<(bool, string)> Eliminar(int id);
        public bool ValidarApuntesDetalleTema(ApuntesDetalleTema apuntesDetalleTema, out string mensajeError);
    }
}
