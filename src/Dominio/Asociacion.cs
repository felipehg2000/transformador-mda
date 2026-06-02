using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Enumeración con las posibles asociaciones entre clases.
    /// </summary>
    public enum EnumTipoAsociacion
    {
        NULO = 0,           /// <summary> Opción para inicializar a vacío los atributos de este tipo. </summary>
        ASOCIACION = 1,     /// <summary> Relación de asociación entre clases </summary>
        AGREGACION = 2,     /// <summary> Relación de agregación entre clases </summary>
        COMPOSICION = 3,    /// <summary> Relación de composición entre clases </summary>    
        HERENCIA = 4,       /// <summary> Relación de herencia entre clases </summary>
        DEPENDENCIA = 5,    /// <summary> Relación de dependencia entre clases </summary>
    }

    /// <summary>
    /// Clase para almacenar toda la información sobre una asociación entre dos clases.
    /// </summary>
    public class Asociacion
    {
        /// <summary>
        /// Almacena la inforamción sobre el tipo de asociación entre las clases.
        /// </summary>
        public EnumTipoAsociacion TipoAsociacion { get; set; }
        
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
            TipoAsociacion = EnumTipoAsociacion.NULO;
            ClaseOrigen = string.Empty;
            ClaseDestino = string.Empty;
            MultiplicidadDestino = string.Empty;
            MultiplicidadOrigen = string.Empty;
        }
    }
}
