using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Clase para almacenar toda la información sobre una asociación entre dos clases.
    /// </summary>
    public class Asociacion : ElementoBase
    {
        public bool EsNavegableOrigen { get; set; }

        public bool EsNavegableDestino { get; set; }

        public EnumTipoAsociacion TipoAsociacion { get; set; }
        
        public string IdClaseOrigen { get; set; }

        public string IdClaseDestino { get; set; }

        public string MultiplicidadOrigen { get; set; }

        public string MultiplicidadDestino { get; set; }

        public string NombreRolOrigen { get; set; }

        public string NombreRolDestino { get; set; }

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
