using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Clase para controlar el tipo de clase leida y a generar.
    /// </summary>
    static public class TiposClases
    {
        /// <summary>
        /// Enumeración, identifica el tipo de clase a generar.
        /// </summary>
        public enum Enumeracion
        {
            NULO = 0,               /// <summary> Inicialización a vacío </summary>
            CLASE = 1,              /// <summary> Indica que el tipo de clase es una clase normal. </summary>
            INTERFAZ = 2,           /// <summary> Indica que el tipo de clase es una interfaz. </summary>
            CLASE_ABSTRACTA = 3,    /// <summary> Indica que el tipo de clase es una clase abstracta. </summary>
            ENUMERACION = 4,        /// <summary> Indica que el tipo de clase es una enumeración. </summary>
        }

        /// <summary>
        /// Obtiene el tipo de clase en enumeración según el texto leído del archivo XMI.
        /// </summary>
        /// <param name="param_tipo_clase"></param>
        /// <param name="param_es_abstracto"></param>
        /// <returns></returns>
        static public Enumeracion ObtenerEnumeracion(string param_tipo_clase, bool param_es_abstracto)
        {
            if (param_tipo_clase == "uml:Interface")
                return Enumeracion.INTERFAZ;
            else if (param_tipo_clase == "uml:Enumeration")
                return Enumeracion.ENUMERACION;
            else if (param_es_abstracto)
                return Enumeracion.CLASE_ABSTRACTA;
            else if (param_tipo_clase == "uml:Class")
                return Enumeracion.CLASE;

            return Enumeracion.NULO;
        }

        /// <summary>
        /// Obtiene el texto a escribir en el archivo XMI según el tipo de clase a generar.
        /// </summary>
        /// <param name="param_tipo_clase">Tipo de clase en enumeración.</param>
        /// <returns>Texto correspondiente al tipo de clase para el archivo XMI.</returns>
        static public string ObtenerTexto(Enumeracion param_tipo_clase)
        {
            switch (param_tipo_clase)
            {
                case Enumeracion.CLASE:
                    return "uml:Class";
                case Enumeracion.INTERFAZ:
                    return "uml:Interface";
                case Enumeracion.CLASE_ABSTRACTA:
                    return "uml:AbstractClass";
                case Enumeracion.ENUMERACION:
                    return "uml:Enumeration";
                default:
                    return string.Empty;
            }
        }
    }
}
