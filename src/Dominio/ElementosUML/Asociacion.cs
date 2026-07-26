using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Clase para almacenar toda la información sobre una asociación entre dos clases.
    /// </summary>
    public class Asociacion : ElementoBase
    {
        /// <summary>
        /// Indica si la asociación es navegable desde la clase origen.
        /// </summary>
        public bool EsNavegableOrigen { get; set; }

        /// <summary>
        /// Indica si la asociación es navegable desde la clase destino.
        /// </summary>
        public bool EsNavegableDestino { get; set; }

        /// <summary>
        /// Indica el tipo de asociación (por ejemplo, agregación, composición, etc.).
        /// </summary>
        public EnumTipoAsociacion TipoAsociacion { get; set; }

        /// <summary>
        /// Identificador de la clase origen de la asociación.
        /// </summary>
        public string IdClaseOrigen { get; set; }

        /// <summary>
        /// Identificador de la clase destino de la asociación.
        /// </summary>
        public string IdClaseDestino { get; set; }

        /// <summary>
        /// Indica la multiplicidad de la asociación desde la clase origen.
        /// </summary>
        public string MultiplicidadOrigen { get; set; }

        /// <summary>
        /// Indica la multiplicidad de la asociación desde la clase destino.
        /// </summary>
        public string MultiplicidadDestino { get; set; }

        /// <summary>
        /// Indica el nombre del rol de la asociación desde la clase origen.
        /// </summary>
        public string NombreRolOrigen { get; set; }

        /// <summary>
        /// Indica el nombre del rol de la asociación desde la clase destino.
        /// </summary>
        public string NombreRolDestino { get; set; }

        /// <summary>
        /// Constructor de la clase Asociacion que inicializa las propiedades con valores predeterminados.
        /// </summary>
        [JsonConstructor]
        public Asociacion() : base() 
        {
            Tipo = EnumTipoElemento.ASOCIACION;
            TipoAsociacion = EnumTipoAsociacion.NO_DEFINIDA;
            IdClaseOrigen = string.Empty;
            IdClaseDestino = string.Empty;
            MultiplicidadOrigen = string.Empty;
            MultiplicidadDestino = string.Empty;
            NombreRolOrigen = string.Empty;
            NombreRolDestino = string.Empty;
        }
    }
}
