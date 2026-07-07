using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    public class Parametro : ElementoBase
    {
        public string Tipado { get; set; }
        
        [JsonConstructor]
        public Parametro() : base()
        {
            Tipo = EnumTipoElemento.PARAMETRO;
            Tipado = string.Empty;
        }
    }
}
