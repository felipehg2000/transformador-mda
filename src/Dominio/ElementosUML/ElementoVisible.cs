using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Representa un elemento visible en un diagrama UML.
    /// </summary>
    public class ElementoVisible : ElementoBase
    {
        /// <summary>
        /// Establece stablece el tipo de visibilidad del elemento visible.
        /// </summary>
        public EnumTipoVisibilidad Visibilidad { get; set; }

        /// <summary>
        /// Constructor de la clase ElementoVisible. Inicializa el tipo de visibilidad como público.
        /// </summary>
        [JsonConstructor]
        public ElementoVisible() : base()
        {
            Visibilidad = EnumTipoVisibilidad.PUBLICO;
        }
    }
}
