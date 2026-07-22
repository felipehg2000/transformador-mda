using System.IO;
using System.Collections.Generic;
using transformador_mda.src.Dominio.ElementosUML;

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
        protected ElementoBase _elemento_actual;

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

        #region "Métodos públicos abstractos"

        /// <summary>
        /// Método inicial de las clases descendientes. Se llama para la generación de la salida.
        /// </summary>
        /// <returns>True si todo es correcto, false si no.</returns>
        public abstract bool GenerarSalida();

        public abstract string GenerarTexto(Clase param_clase);
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

        protected abstract string GenerarAtributos(List<Atributo> param_elementos);

        protected abstract string GenerarParametros(List<Parametro> param_parametros);

        /// <summary>
        /// Método encargado de generar los métodos que se añadirán a la clase.
        /// </summary>
        /// <param name="param_metodos">Método a generar.</param>
        /// <returns>Código del método.</returns>
        protected abstract string GenerarMetodos(List<Metodo> param_metodos);
        
        #endregion
    }
}
