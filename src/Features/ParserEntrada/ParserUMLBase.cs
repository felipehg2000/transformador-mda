using transformador_mda.src.Dominio.ElementosUML;

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
        /// Método para procesar un directorio, se llama desde ProcesarElemento si el elemento es un directorio.
        /// </summary>
        /// <returns>Objeto de tipo Directorio con los datos del directorio</returns>
        protected abstract Directorio ProcesarDirectorio();

        /// <summary>
        /// Método para procesar una clase, se llama desde ProcesarElemento si el elemento es una clase.
        /// </summary>
        /// <returns>Objeto de tipo Clase con los datos de la clase</returns>
        protected abstract Clase ProcesarClase();

        /// <summary>
        /// Método para procesar un atributo, se llama desde ProcesarElemento si el elemento es un atributo.
        /// </summary>
        /// <returns>Objeto de tipo Atributo con los datos del atributo.</returns>
        protected abstract Atributo ProcesarAtributo();

        /// <summary>
        /// Método para procesar un método, se llama desde ProcesarElemento si el elemento es un método.
        /// </summary>
        /// <returns>Objeto de tipo Metodo con los datos del método.</returns>
        protected abstract Metodo ProcesarMetodo();

        /// <summary>
        /// Método para procesar un parámetro, se llama desde ProcesarElemento si el elemento es un parámetro.
        /// </summary>
        /// <returns>Objeto de tipo Parametro con los datos del parámetro.</returns>
        protected abstract Parametro ProcesarParametro();

        /// <summary>
        /// Método para procesar una asociación, se llama desde ProcesarElemento si el elemento es una asociación.
        /// </summary>
        /// <returns>Objeto de tipo Asociacion con los datos de la asociación.</returns>
        protected abstract Asociacion ProcesarAsociacion();
    }
}
