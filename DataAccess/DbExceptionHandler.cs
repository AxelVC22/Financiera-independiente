using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.DataAccess
{
    public static class DbExceptionHandler
    {
        public static InvalidOperationException Handle(Exception ex)
        {
            string message;

            if (ex is DbEntityValidationException)
            {
                message = "No se pudo guardar la información. Por favor, verifica que todos los campos estén completos y correctos.";
            }
            else if (ex is EntityException)
            {
                message = "Ocurrió un error al acceder a los datos. Inténtalo nuevamente más tarde.";
            }
            else if (ex is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
            {
                message = "El registro ya existe. Verifica que los datos no estén duplicados.";
            }
            else if (ex is SqlException)
            {
                message = "No se pudo completar la operación en la base de datos. Inténtalo de nuevo más tarde.";
            }
            else
            {
                message = "Ocurrió un error inesperado. Por favor, inténtalo nuevamente.";
            }

            return new InvalidOperationException(message, ex); 
        }
    }
}
