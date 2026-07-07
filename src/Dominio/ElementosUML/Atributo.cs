using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    public class Atributo : ElementoVisible
    {
        public string Tipado { get; set; }

        [JsonConstructor]
        public Atributo() : base()
        {
            Tipo = EnumTipoElemento.ATRIBUTO;
            Tipado = string.Empty;
        }
    }
}
