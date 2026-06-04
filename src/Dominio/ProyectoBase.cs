using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Clase para almacenar la información sobre la estructura completa del proyecto a generar
    /// </summary>
    public class ProyectoBase
    {
        /// <summary>
        /// Identificador único del proyecto.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nombre del proyecto
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Directorio raíz del proyecto, a partir del cual se generará toda la estructura de directorios y archivos.
        /// </summary>
        public string DirectorioRaiz { get; set; }

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
            Id = string.Empty;
            Nombre = string.Empty;
            DirectorioRaiz = string.Empty;
            Directorios = new List<Directorio>();
        }

        /// <summary>
        /// Método para obtener un directorio por su identificador único.
        /// </summary>
        /// <param name="param_id">Identificador del directorio a buscar.</param>
        /// <returns>El directorio encontrado o null si no existe.</returns>
        public Directorio ObtenerDirectorioPorId(string param_id)
        {
            return Directorios.Find(directorio => directorio.Id == param_id);
        }
    }
}
