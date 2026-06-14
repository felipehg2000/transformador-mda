using System.IO;
using System.Collections.Generic;
using transformador_mda.src.Dominio;

namespace transformador_mda.src.Features.GenerarSalida
{
    /// <summary>
    /// Clase padre para la generación de salidas. Habilita el polimorfismo para la factoría.
    /// </summary>
    public abstract class GenerarSalidaBase
    {
        #region "Atributos"

        /// <summary>
        /// Proyecto del cual se generará la salida. Contiene toda la información sobre el proyecto a generar.
        /// </summary>
        protected ProyectoBase _proyecto;

        /// <summary>
        /// Directorio que se está procesando en el bucle que recorre la representación interna.
        /// </summary>
        protected Directorio _directorio_actual;

        /// <summary>
        /// Clase que se está procesando en el bucle que recorre la representación interna.
        /// </summary>
        protected Clase _clase_actual;

        /// <summary>
        /// Elemento que se está procesando en el bucle que recorre la representación interna.
        /// </summary>
        protected Elemento _elemento_actual;

        /// <summary>
        /// Asociación que se está procesando en el bucle que recorre la representación interna.
        /// </summary>
        protected Asociacion _asociacion_actual;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Constructor de la clase, inicializa atributos.
        /// </summary>
        /// <param name="param_proyecto"></param>
        public GenerarSalidaBase(ProyectoBase param_proyecto)
        {
            _proyecto = param_proyecto;
        }

        #endregion

        #region "Métodos protegidos"

        /// <summary>
        /// Genera la estructura de directorios de un sistema.
        /// </summary>
        /// <returns>True si la estructura de directorios se generó correctamente, false en caso contrario.</returns>
        protected bool GenerarEstructuraDirectorios()
        {
            if (_proyecto == null ||
                string.IsNullOrEmpty(_proyecto.DirectorioRaiz))
                return false;

            if (!Directory.Exists(_proyecto.DirectorioRaiz))
                Directory.CreateDirectory(_proyecto.DirectorioRaiz);

            foreach (var directorio in _proyecto.Directorios)
            {
                var path = Path.Combine(_proyecto.DirectorioRaiz, directorio.Nombre);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            return true;
        }

        #endregion

        #region "Métodos públicos abstractos"

        /// <summary>
        /// Método inicial de las clases descendientes. Se llama para la generación de la salida.
        /// </summary>
        /// <returns>True si todo es correcto, false si no.</returns>
        public abstract bool GenerarSalida();

        #endregion

        #region "Métodos protegidos abstractos"

        /// <summary>
        /// Método encargado de generar las clases.
        /// </summary>
        protected abstract void GenerarClases();

        /// <summary>
        /// Método encargado de generar el códgio con el que se rellenarán las clases.
        /// </summary>
        /// <returns>Código de la clase.</returns>
        protected abstract string GenerarCodigoClase();

        /// <summary>
        /// Método encargado de generar el código de los elementos que se añadirán a las clases.
        /// </summary>
        /// <param name="param_elementos">Elemento a generar.</param>
        /// <returns>Código del elemento.</returns>
        protected abstract string GenerarElementos(List<Elemento> param_elementos);

        /// <summary>
        /// Método encargado de generar los métodos que se añadirán a la clase.
        /// </summary>
        /// <param name="param_metodos">Método a generar.</param>
        /// <returns>Código del método.</returns>
        protected abstract string GenerarMetodos(List<Metodo> param_metodos);
        
        #endregion
    }
}
