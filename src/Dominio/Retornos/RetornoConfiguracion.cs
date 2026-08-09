using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.Retornos
{
    /// <summary>
    /// Representa el retorno de una operación que devuelve la configuración del sistema.
    /// </summary>
    public class RetornoConfiguracion : RetornoBase
    {
        /// <summary>
        /// Establece la configuración del sistema que se devuelve como resultado de la operación.
        /// </summary>
        public Configuracion Configuracion { get; set; }

        /// <summary>
        /// Constructor, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public RetornoConfiguracion() : base()
        {
            Configuracion = new Configuracion();
        }

        /// <summary>
        /// Constructor, inicializa atributos.
        /// </summary>
        /// <param name="param_exito">True si la operación fue exitosa, false en caso contrario</param>
        /// <param name="param_texto_usuario">Mensaje para el usuario</param>
        /// <param name="param_configuracion">Configuración del sistema que se devuelve</param>
        /// <param name="param_texto_desarrollador">Mensaje para el desarrollador</param>
        public RetornoConfiguracion(bool param_exito,
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
