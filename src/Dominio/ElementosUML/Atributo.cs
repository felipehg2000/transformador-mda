using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Clase para almacenar toda la información sobre un atributo de una clase en un diagrama UML.
    /// </summary>
    public class Atributo : ElementoVisible
    {
        /// <summary>
        /// Indica el tipo de dato de un atributo.
        /// </summary>
        public EnumTipoDato TipoDato { get; set; }

        /// <summary>
        /// Constructor de la clase, incializa atributos.
        /// </summary>
        [JsonConstructor]
        public Atributo() : base()
        {
            Tipo = EnumTipoElemento.ATRIBUTO;
            TipoDato = EnumTipoDato.NO_DEFINIDO;
        }
    }
}
