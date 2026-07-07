using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Clase para almacenar toda la información sobre un parámetro de un método en un diagrama UML.
    /// </summary>
    public class Parametro : ElementoBase
    {
        /// <summary>
        /// Indica el tipo de dato del parámetro (por ejemplo, int, string, etc.).
        /// </summary>
        public string TipoDato { get; set; }

        /// <summary>
        /// Constructor de la clase, incializa atributos.
        /// </summary>
        [JsonConstructor]
        public Parametro() : base()
        {
            Tipo = EnumTipoElemento.PARAMETRO;
            TipoDato = string.Empty;
        }
    }
}
