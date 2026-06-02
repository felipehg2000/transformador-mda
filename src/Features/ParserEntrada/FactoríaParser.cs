using System;
using transformador_mda.src.Dominio;

namespace transformador_mda.src.Features.ParserEntrada
{
    /// <summary>
    /// Factoría para crear el parser de entrada según el tipo de fichero de entrada.
    /// </summary>
    public class FactoríaParser
    {
        /// <summary>
        /// Crea un parser de entrada según el tipo de fichero de entrada.
        /// </summary>
        /// <param name="param_fichero_entrada">Tipo de fichero de entrada.</param>
        /// <returns>Instancia del parser de entrada correspondiente.</returns>
        /// <exception cref="ArgumentNullException">Se lanza cuando el tipo de fichero de entrada es nulo.</exception>
        /// <exception cref="ArgumentException">Se lanza cuando el tipo de fichero de entrada no está soportado.</exception>
        public static IParserEntrada CrearrParser(Configuracion param_configuracion)
        {
            switch (param_configuracion.TipoFicheroEntrada)
            {
                case EnumTipoFicheroEntrada.XMI:
                    return new ParserXMI(param_configuracion.RutaFicheroEntrada);
                case EnumTipoFicheroEntrada.NULO:
                    throw new ArgumentNullException($"El tipo de fichero de entrada no puede ser nulo.");
                default:
                    throw new ArgumentException($"Tipo de parser no soportado.");
            }
        }
    }
}
