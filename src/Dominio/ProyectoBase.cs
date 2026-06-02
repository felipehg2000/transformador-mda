using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Clase para almacenar la información sobre la estructura completa del proyecto a generar
    /// </summary>
    internal class ProyectoBase
    {
        /// <summary>
        /// Nombre del proyecto
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Lista de directorios
        /// </summary>
        public List<Directorio> Directorios { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public ProyectoBase()
        {
            Nombre = string.Empty;
            Directorios = new List<Directorio>();
        }
    }
}
