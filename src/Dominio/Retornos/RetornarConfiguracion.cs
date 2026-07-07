using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.Retornos
{
    public class RetornarConfiguracion : RetornoBase
    {
        public Configuracion Configuracion { get; set; }

        [JsonConstructor]
        public RetornarConfiguracion(bool param_exito, 
            string param_texto_usuario,
            Configuracion param_configuracion,
            string param_texto_desarrollador = null) : base(param_exito,
                param_texto_usuario, 
                param_texto_desarrollador)
        {
            Configuracion = param_configuracion;
        }
    }
}
