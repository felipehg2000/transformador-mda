using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transformador_mda.src.Features.CrearIds
{
    /// <summary>
    /// Clase para generar distintos tipos de identificadores.
    /// </summary>
    public class CrearIdentificador
    {
        /// <summary>
        /// Método para generar GUIDs
        /// </summary>
        /// <returns>GUID específico.</returns>
        static public string CrearGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
