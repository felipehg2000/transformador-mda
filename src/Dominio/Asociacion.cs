using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Clase para almacenar toda la información sobre una asociación entre dos clases.
    /// </summary>
    public class Asociacion
    {
        /// <summary>
        /// Identificador único de la asociación.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Almacena la inforamción sobre el tipo de asociación entre las clases.
        /// </summary>
        public TiposAsociaciones.Enumeracion TipoAsociacion { get; set; }
        
        /// <summary>
        /// Nombre de la clase origen de la relación.
        /// </summary>
        public string ClaseOrigen { get; set; }

        /// <summary>
        /// Nombre de la clase destino de la relación.
        /// </summary>
        public string ClaseDestino { get; set; }
    
        /// <summary>
        /// Multiplicidad de la clase origen de la relación.
        /// </summary>
        public string MultiplicidadOrigen { get; set; }

        /// <summary>
        /// Multiplicidad de la clase destino de la relación.
        /// </summary>
        public string MultiplicidadDestino { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa los atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public Asociacion()
        {
            Id = string.Empty;
            TipoAsociacion = TiposAsociaciones.Enumeracion.NULO;
            ClaseOrigen = string.Empty;
            ClaseDestino = string.Empty;
            MultiplicidadDestino = string.Empty;
            MultiplicidadOrigen = string.Empty;
        }
    }
}
