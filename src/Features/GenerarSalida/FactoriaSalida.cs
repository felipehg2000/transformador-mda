using System;
using transformador_mda.src.Dominio;

namespace transformador_mda.src.Features.GenerarSalida
{
    /// <summary>
    /// Factoría para crear el generador de salida según el tipo de salida a generar.
    /// </summary>
    public class FactoriaSalida
    {
        /// <summary>
        /// Crea una instancia de un generador de salida basado en la configuración proporcionada.
        /// </summary>
        /// <param name="param_configuracion">Configuración del proceso de transformación.</param>
        /// <returns>Instancia de un generador de salida.</returns>
        /// <exception cref="ArgumentNullException">Se lanza si el tipo de salida es nulo.</exception>
        /// <exception cref="ArgumentException">Se lanza si el tipo de salida no es soportado.</exception>
        public static GenerarSalidaBase CrearGenerarSalida(Configuracion param_configuracion)
        {
            switch (param_configuracion.TipoSalida)
            {
                case EnumTipoSalida.C_SHARP:
                    return new C_Sharp(param_configuracion.Proyecto);
                case EnumTipoSalida.NULO:
                    throw new ArgumentNullException($"El tipo de fichero de salida no puede ser nulo.");
                default:
                    throw new ArgumentException($"Tipo de generador de salida no soportado.");
            }
        }
    }
}
