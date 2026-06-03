using System;

namespace transformador_mda.src.Dominio
{
    static public class TiposAsociaciones
    {
        /// <summary>
        /// Enumeración con las posibles asociaciones entre clases.
        /// </summary>
        public enum Enumeracion
        {
            NULO = 0,           /// <summary> Opción para inicializar a vacío los atributos de este tipo. </summary>
            ASOCIACION = 1,     /// <summary> Relación de asociación entre clases </summary>
            AGREGACION = 2,     /// <summary> Relación de agregación entre clases </summary>
            COMPOSICION = 3,    /// <summary> Relación de composición entre clases </summary>    
            HERENCIA = 4,       /// <summary> Relación de herencia entre clases </summary>
            DEPENDENCIA = 5,    /// <summary> Relación de dependencia entre clases </summary>
        }

        static public Enumeracion ObtenerTipoAsociacion(string tipo_asociacion)
        {
            switch (tipo_asociacion.ToUpper())
            {
                case "UML:ASSOCIATION":
                    return Enumeracion.ASOCIACION;
                case "SHARED":
                    return Enumeracion.AGREGACION;
                case "COMPOSITE":
                    return Enumeracion.COMPOSICION;
                case "GENERALIZATION":
                    return Enumeracion.HERENCIA;
                case "UML:DEPENDENCY":
                    return Enumeracion.DEPENDENCIA;
                default:
                    throw new Exception("El tipo de asociación no es válido.");
            }
        }

        static public string ObtenerNombreTipoAsociacion(Enumeracion tipo_asociacion)
        {
            switch (tipo_asociacion)
            {
                case Enumeracion.ASOCIACION:
                    return "uml:Association";
                case Enumeracion.AGREGACION:
                    return "shared";
                case Enumeracion.COMPOSICION:
                    return "composite";
                case Enumeracion.HERENCIA:
                    return "generalization";
                case Enumeracion.DEPENDENCIA:
                    return "uml:Dependency";
                default:
                    throw new Exception("El tipo de asociación no es válido.");
            }
        }
    }       
}
