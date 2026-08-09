using System.Text.Json.Serialization;
using transformador_mda.src.Dominio.ElementosUML;

namespace transformador_mda.src.Dominio.Retornos
{
    /// <summary>
    /// Representa el retorno de una operación que devuelve un elemento UML.
    /// </summary>
    public class RetornoElementoUML : RetornoBase
    {
        /// <summary>
        /// Establce el elemento UML que se devuelve como resultado de la operación.
        /// </summary>
        public ElementoBase Elemento { get; set; }

        /// <summary>
        /// Constructor, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public RetornoElementoUML() : base()
        {
            Elemento = new ElementoBase();
        }

        /// <summary>
        /// Constructor, inicializa atributos a vacío.
        /// </summary>
        /// <param name="param_exito">True si la operación fue exitosa, false en caso contrario</param>
        /// <param name="param_texto_usuario">Mensaje para el usuario</param>
        /// <param name="param_elemento">Elemento UML que se devuelve</param>
        /// <param name="param_texto_desarrollador">Mensaje para el desarrollador</param>
        public RetornoElementoUML(bool param_exito,
            string param_texto_usuario,
            ElementoBase param_elemento,
            string param_texto_desarrollador = null) :
            base(param_exito,
                param_texto_usuario,
                param_texto_desarrollador)
        {
            Elemento = param_elemento;
        }
    }
}
