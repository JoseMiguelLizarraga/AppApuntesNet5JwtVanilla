using System;

namespace Util
{
    public static class ExcepcionesHelper
    {

        public static ExcepcionCapturada ObtenerExcepcion(Exception ex)
        {
            string mensajeError = ex.InnerException.Message;

            if (ex.InnerException != null)
            {
                if (!string.IsNullOrEmpty(ex.InnerException.Message))
                    mensajeError = ex.InnerException.Message;
                
                else if (ex.InnerException.InnerException != null)
                    mensajeError = ex.InnerException.InnerException.Message;
                
                else
                    mensajeError = ex.Message;
            }

            ExcepcionCapturada excepcion = new ExcepcionCapturada();
            excepcion.MensajeError = mensajeError;
            excepcion.Status = 500;

            return excepcion;
        }

        public static ExcepcionCapturada GenerarExcepcion(string mensajeError, int status)
        {

            ExcepcionCapturada excepcion = new ExcepcionCapturada();
            excepcion.MensajeError = mensajeError;
            excepcion.Status = status;

            return excepcion;
        }
    }
}
