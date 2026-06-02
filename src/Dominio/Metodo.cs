using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Clase para almacenar la información sobre una operación.
    /// </summary>
    public class Metodo
    {
        /// <summary>
        /// Nombre de la operación.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Tipo de dato que retorna el método (bool, int, void...)
        /// </summary>
        public string TipoRetorno { get; set; }

        /// <summary>
        /// Visibilidad de la operación.
        /// </summary>
        public EnumVisibilidad Visibilidad { get; set; }

        /// <summary>
        /// Parámetros asociados al método.
        /// </summary>
        public List<Elemento> Parametros { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos.
        /// </summary>
        [JsonConstructor]
        public Metodo()
        {
            Nombre = string.Empty;
            TipoRetorno = string.Empty;
            Visibilidad = EnumVisibilidad.NULO;
            Parametros = new List<Elemento>();
        }
    }
}
