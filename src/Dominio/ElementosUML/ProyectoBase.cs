using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Clase para almacenar la información sobre la estructura completa del proyecto a generar
    /// </summary>
    public class ProyectoBase : ElementoBase
    {
        /// <summary>
        /// Lista de directorios
        /// </summary>
        public List<Directorio> Directorios { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public ProyectoBase() : base()
        {
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
