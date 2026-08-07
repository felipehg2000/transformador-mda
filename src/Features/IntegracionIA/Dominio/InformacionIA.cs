using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Features.IntegracionIA.Dominio
{
    internal class InformacionIA
    {
        internal string RutaDirectorioFicheros { get; set; }

        internal InformacionFichero InformacionJSON { get; set; }

        internal List<InformacionFichero> InformacionFicherosEsqueleto { get; set; }

        [JsonConstructor]
        public InformacionIA()
        {
            RutaDirectorioFicheros = string.Empty;
            InformacionJSON = new InformacionFichero();
            InformacionFicherosEsqueleto = new List<InformacionFichero>();
        }
    }
}
