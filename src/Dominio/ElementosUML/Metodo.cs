using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Clase para almacenar la información sobre una operación.
    /// </summary>
    public class Metodo : ElementoVisible
    {
        /// <summary>
        /// Tipo de dato que retorna el método (bool, int, void...)
        /// </summary>
        public string TipoRetorno { get; set; }

        /// <summary>
        /// Parámetros asociados al método.
        /// </summary>
        public List<Elemento> Parametros { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos.
        /// </summary>
        [JsonConstructor]
        public Metodo() : base()
        {
            TipoRetorno = string.Empty;
            Parametros = new List<Elemento>();
        }

        /// <summary>
        /// Busca un elemento dentro de la lista de parámetros por su identificador.
        /// </summary>
        /// <param name="param_id">Identificador del elemento a buscar.</param>
        /// <returns>El elemento encontrado o null si no existe.</returns>
        public Elemento BuscarElementoPorId(string param_id)
        {
            return Parametros.Find(elemento => elemento.Id == param_id);
        }
    }
}
