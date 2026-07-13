using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;

namespace transformador_mda.src.Features.ParserEntrada
{
    /// <summary>
    /// Método padre para habilitar el polimorfismo e implementar el patrón factory de parsers.
    /// </summary>
    public abstract class ParserUMLBase
    {
        /// <summary>
        /// Ruta del fichero a procesar.
        /// </summary>
        protected string _ruta_fichero;

        /// <summary>
        /// Constructor, inicializa atributos.
        /// </summary>
        /// <param name="param_ruta_fichero">Ruta del fichero a parsear</param>
        public ParserUMLBase(string param_ruta_fichero)
        {
            _ruta_fichero = param_ruta_fichero;
        }

        /// <summary>
        /// Método principal para los descendientes. Se llama a este método para hacer el análisis sintáctico.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso de lectura y errores, si los hubiera.</returns>
        public abstract RetornoElementoUML Parsear();
    }
}
