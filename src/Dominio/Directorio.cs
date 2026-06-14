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
        /// Identificador único del directorio.
        /// </summary>
        public string Id { get; set; }

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
            Id = string.Empty;
            Nombre = string.Empty;
            Namespace = string.Empty;
            Clases = new List<Clase>();
        }

        /// <summary>
        /// Obtiene una clase del directorio a partir de su identificador.
        /// </summary>
        /// <param name="param_id">Identificador de la clase a buscar.</param>
        /// <returns>La clase encontrada o null si no existe.</returns>
        public Clase ObtenerClasePorId(string param_id)
        {
            return Clases.Find(clase => clase.Id == param_id);
        }
    }
}
