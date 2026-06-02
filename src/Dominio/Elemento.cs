using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Enumeración con los tipos de elementos que se pueden definir.
    /// </summary>
    public enum EnumTipoElemento
    {
        NULO = 0,           /// <summary> Elemento vacío, para inicializar atributos a vacío. </summary>
        ATRIBUTO = 1,       /// <summary> Representa un elemento de tipo atributo. </summary>
        PROPIEDAD = 2,      /// <summary> Representa un elemento de tipo propiedad. </summary>
    }

    /// <summary>
    /// Clase para almacenar la información sobre un elemento.
    /// </summary>
    public class Elemento
    {
        /// <summary>
        /// Nombre del atributo o la propiedad.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Tipo de dato del atributo o propiedad (bool, int, string...)
        /// </summary>
        public string Tipo { get; set; }
        
        /// <summary>
        /// Tipo de elemento, para diferenciar entre atributos y propiedades.
        /// </summary>
        public EnumTipoElemento TipoElemento { get; set; }

        /// <summary>
        /// Visibilidad del atributo (publico, privado...)
        /// </summary>
        /// <remarks>
        /// Solo es útil para los atributos, en las propiedades el valor será EnumVisibilidad.NULO.
        /// </remarks>
        public EnumVisibilidad Visibilidad { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public Elemento()
        {
            Nombre = string.Empty;
            Tipo = string.Empty;
            TipoElemento = EnumTipoElemento.NULO;
            Visibilidad = EnumVisibilidad.NULO;
        }
    }
}
