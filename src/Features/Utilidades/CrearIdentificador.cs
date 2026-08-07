using System;

namespace transformador_mda.src.Features.Utilidades
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
