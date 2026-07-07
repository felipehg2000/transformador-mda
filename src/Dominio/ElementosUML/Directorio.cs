using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Clase para almacenar la información sobre un directorio.
    /// </summary>
    public class Directorio : ElementoBase
    {
        /// <summary>
        /// Espacio de nombres asociado al directorio.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Clases que engloba el directorio
        /// </summary>
        public List<Clase> Clases { get; set; }

        /// <summary>
        /// Subdirectorios que engloba el directorio.
        /// </summary>
        public List<Directorio> SubDirectorios { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>        
        [JsonConstructor]
        public Directorio() : base()
        {
            Tipo = EnumTipoElemento.DIRECTORIO;
            Namespace = string.Empty;
            SubDirectorios = new List<Directorio>();
            Clases = new List<Clase>();
        }

        /// <summary>
        /// Obtiene un subdirectorio a partir de su identificador.
        /// </summary>
        /// <param name="param_id">Identificador del subdirectorio a buscar.</param>
        /// <returns>El subdirectorio encontrado o null si no existe.</returns>
        public Directorio ObtenerDirectorioPorID(string param_id)
        {
            return SubDirectorios.Find(directorio => directorio.Id == param_id);
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
