using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Clase para almacenar la información sobre un directorio.
    /// </summary>
    public class Directorio
    {
        /// <summary>
        /// Nombre de la carpeta.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Espacio de nombres asociado al directorio.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Clases que engloba el directorio
        /// </summary>
        public List<Clase> Clases { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>        
        [JsonConstructor]
        public Directorio()
        {
            Nombre = string.Empty;
            Namespace = string.Empty;
            Clases = new List<Clase>();
        }
    }
}
