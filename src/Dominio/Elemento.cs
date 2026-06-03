using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Clase para almacenar la información sobre un elemento.
    /// </summary>
    public class Elemento
    {
        /// <summary>
        /// Identificador único del elemento.
        /// </summary>
        public string Id { get; set; }

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
        public TiposElementos.Enumeracion TipoElemento { get; set; }

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
            Id = string.Empty;
            Nombre = string.Empty;
            Tipo = string.Empty;
            TipoElemento = TiposElementos.Enumeracion.NULO;
            Visibilidad = EnumVisibilidad.NULO;
        }
    }
}
