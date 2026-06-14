using transformador_mda.src.Dominio;

namespace transformador_mda.src.Features.ParserEntrada
{
    /// <summary>
    /// Método padre para habilitar el polimorfismo e implementar el patrón factory de parsers.
    /// </summary>
    public abstract class IParserEntrada
    {
        /// <summary>
        /// Ruta del fichero a procesar.
        /// </summary>
        protected string _ruta_fichero;

        /// <summary>
        /// Constructor, inicializa atributos.
        /// </summary>
        /// <param name="param_ruta_fichero">Ruta del fichero a parsear</param>
        public IParserEntrada(string param_ruta_fichero)
        {
            _ruta_fichero = param_ruta_fichero;
        }

        /// <summary>
        /// Método principal para los descendientes. Se llama a este método para hacer el análisis sintáctico.
        /// </summary>
        /// <returns>Objeto de tipo ProyectoBase con los datos del documento si todo ha ido bien o null si ha fallado algo.</returns>
        public abstract ProyectoBase Parsear();

        /// <summary>
        /// Método para comprobar si un archivo es procesable o no.
        /// </summary>
        /// <returns>True si el generador soporta el fichero, false si no.</returns>
        protected abstract bool EsFormatoValido();

        /// <summary>
        /// Método para hacer el bucle de lectura, recorre elemento a elemento, comprueba si el elemento es procesable y si lo es llama a ProcesarElemento.
        /// </summary>
        protected abstract void LeerFichero();

        /// <summary>
        /// Factoría clásica, según el método que se vaya a procesar toma la decisión de qué función debe ser ejecutada.
        /// </summary>
        protected abstract void ProcesarElemento();

        /// <summary>
        /// Método encargado de procesar los directorios
        /// </summary>
        protected abstract void ProcesarPaquete();

        /// <summary>
        /// Método encargado de procesar las clases
        /// </summary>
        protected abstract void ProcesarClase();

        /// <summary>
        /// Método encargado de procesar los atributos, parámetros y propiedades.
        /// </summary>
        protected abstract void ProcesarAtributoParametro();

        /// <summary>
        /// Método encargado de procesar los métodos.
        /// </summary>
        protected abstract void ProcesarMetodo();

        /// <summary>
        /// Método encargado de procesar las asociaciones.
        /// </summary>
        protected abstract void ProcesarAsociacion();
    }
}
