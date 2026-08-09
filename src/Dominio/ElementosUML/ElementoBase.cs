using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Clase base que representa un elemento UML genérico. Contiene propiedades comunes a todos los elementos UML, como Id, Nombre, Comentario y Tipo.
    /// </summary>
    public class ElementoBase
    {
        /// <summary>
        /// Identificador único del elemento UML.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nombre del elemento UML.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Comentario o descripción funcional del elemento UML.
        /// </summary>
        public string Comentario { get; set; }

        /// <summary>
        /// Tipo de elemento UML, representado por la enumeración EnumTipoElemento.
        /// </summary>
        public EnumTipoElemento Tipo { get; set; }

        /// <summary>
        /// Constructor por defecto de la clase ElementoBase, inicializa los atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public ElementoBase()
        {
            Id = string.Empty;
            Nombre = string.Empty;
            Comentario = string.Empty;
            Tipo = EnumTipoElemento.NO_DEFINIDO;
        }
    }
}
