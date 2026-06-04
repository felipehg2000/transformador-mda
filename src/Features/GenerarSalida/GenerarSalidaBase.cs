using System.Collections.Generic;
using System.IO;
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

        protected Directorio _directorio_actual;

        protected Clase _clase_actual;

        protected Elemento _elemento_actual;

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

        public abstract bool GenerarSalida();

        #endregion

        #region "Métodos protegidos abstractos"

        protected abstract void GenerarClases();

        protected abstract string GenerarCodigoClase();

        protected abstract string GenerarElementos(List<Elemento> param_elementos);

        protected abstract string GenerarMetodos(List<Metodo> param_metodos);
        
        #endregion
    }
}
