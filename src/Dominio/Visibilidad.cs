using System; 

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Enumeración para indicar los diferentes tipos de visibilidad de los artefactos software a generar.
    /// </summary>
    public enum EnumVisibilidad
    {
        NULO = 0,           /// <summary> Valor para inicializar a vacío los atributos. </summary>
        PUBLICO = 1,        /// <summary> Indica una visibilidad de tipo public para clases, métodos, propiedades y atributos.</summary>
        PRIVADO = 2,        /// <summary> Indica una visibilidad de tipo private para clases, métodos, propiedades y atributos.</summary>
        PROTEGIDO = 3,      /// <summary> Indica una visibilidad de tipo protected para clases, métodos, propiedades y atributos.</summary>
        INTERNA = 4,        /// <summary> Indica una visibilidad de tipo internal para clases. </summary>
    }

    static public class Visibilidad
    {
        /// <summary>
        /// Método que devuelve el tipo de visibilidad asociado a un valor de la enumeración.
        /// </summary>
        /// <param name="param_enum_visibilidad">Valor de la enumeración para el cual se desea obtener el tipo de visibilidad.</param>
        /// <returns>Tipo de visibilidad asociado al valor de la enumeración.</returns>
        /// <exception cref="ArgumentException">Se lanza si el valor de la enumeración no está soportado.</exception>
        static public string ObtenerVisibilidad(EnumVisibilidad param_enum_visibilidad)
        {
            switch (param_enum_visibilidad)
            {
                case EnumVisibilidad.PUBLICO:
                    return "public";
                case EnumVisibilidad.PRIVADO:
                    return "private";
                case EnumVisibilidad.PROTEGIDO:
                    return "protected";
                case EnumVisibilidad.INTERNA:
                    return "internal";
                default:
                    throw new ArgumentException("Valor de enum no soportado");
            }
        }

        /// <summary>
        /// Método que devuelve el valor de la enumeración asociado a un tipo de visibilidad.
        /// </summary>
        /// <param name="param_visibilidad">Tipo de visibilidad en formato string.</param>
        /// <returns>Valor de la enumeración correspondiente al tipo de visibilidad.</returns>
        /// <exception cref="ArgumentException">Se lanza si el tipo de visibilidad no está soportado.</exception>
        static public EnumVisibilidad ObtenerEnumeracion(string param_visibilidad)
        {
            switch (param_visibilidad)
            {
                case "public":
                    return EnumVisibilidad.PUBLICO;
                case "private":
                    return EnumVisibilidad.PRIVADO;
                case "protected":
                    return EnumVisibilidad.PROTEGIDO;
                case "internal":
                    return EnumVisibilidad.INTERNA;
                default:
                    throw new ArgumentException("Valor de visibilidad no soportado");
            }
        }
    }
}
