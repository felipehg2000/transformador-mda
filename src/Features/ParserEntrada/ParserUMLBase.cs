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

        /*
        /// <summary>
        /// Método para comprobar si un archivo es procesable o no.
        /// </summary>
        /// <returns>True si el generador soporta el fichero, false si no.</returns>
        protected abstract bool EsFormatoValido();

        /// <summary>
        /// Método para hacer el bucle de lectura, recorre elemento a elemento, comprueba si el elemento es procesable y si lo es llama a ProcesarElemento.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso de lectura y errores, si los hubiera.</returns>
        protected abstract RetornoElementoUML LeerFichero();

        /// <summary>
        /// Factoría clásica, según el método que se vaya a procesar toma la decisión de qué función debe ser ejecutada.
        /// </summary>
        /// <param name="param_es_subdirectorio">Indica si el elemento es un subdirectorio o no.</param>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso de lectura y errores, si los hubiera.</returns>
        protected abstract bool ProcesarElemento(bool param_es_subdirectorio = false);

        /// <summary>
        /// Método para procesar un directorio, se llama desde ProcesarElemento si el elemento es un directorio.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso de lectura y errores, si los hubiera.</returns>
        protected abstract RetornoElementoUML ProcesarDirectorio();

        /// <summary>
        /// Método para procesar una clase, se llama desde ProcesarElemento si el elemento es una clase.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso de lectura y errores, si los hubiera.</returns>
        protected abstract RetornoElementoUML ProcesarClase();

        /// <summary>
        /// Método para procesar un atributo, se llama desde ProcesarElemento si el elemento es un atributo.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso de lectura y errores, si los hubiera.</returns>
        protected abstract RetornoElementoUML ProcesarAtributo();

        /// <summary>
        /// Método para procesar un método, se llama desde ProcesarElemento si el elemento es un método.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso de lectura y errores, si los hubiera.</returns>
        protected abstract RetornoElementoUML ProcesarMetodo();

        /// <summary>
        /// Método para procesar un parámetro, se llama desde ProcesarElemento si el elemento es un parámetro.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso de lectura y errores, si los hubiera.</returns>
        protected abstract RetornoElementoUML ProcesarParametro();

        /// <summary>
        /// Método para procesar una asociación, se llama desde ProcesarElemento si el elemento es una asociación.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso de lectura y errores, si los hubiera.</returns>
        protected abstract RetornoElementoUML ProcesarAsociacion();
        */
    }
}
