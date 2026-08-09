using System.Text.Json.Serialization;

namespace transformador_mda.src.Features.IntegracionIA.Dominio
{
    internal class InformacionFichero
    {
        internal string IdClaseDelEsqueleto { get; set; }

        internal string RutaFichero { get; set; }

        internal string IdFicheroOpenAI { get; set; }

        [JsonConstructor]
        public InformacionFichero()
        {
            IdClaseDelEsqueleto = string.Empty;
            RutaFichero = string.Empty;
            IdFicheroOpenAI = string.Empty;
        }

        public InformacionFichero(string param_ruta_fichero, string param_id_fichero_openai, string param_id_clase_esqueleto = null)
        {
            RutaFichero = param_ruta_fichero;
            IdFicheroOpenAI = param_id_fichero_openai;
            IdClaseDelEsqueleto = param_id_clase_esqueleto ?? string.Empty;
        }
    }
}
