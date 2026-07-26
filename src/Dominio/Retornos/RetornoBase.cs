using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.Retornos
{
    /// <summary>
    /// Representa el retorno de una operación con información básica sobre el resultado de la operación.
    /// </summary>
    public class RetornoBase
    {
        /// <summary>
        /// Indica si la operación fue exitosa (true) o no (false).
        /// </summary>
        public bool Exito { get; set; }

        /// <summary>
        /// Mensaje para el usuario que describe el resultado de la operación.
        /// </summary>
        public string TextoUsuario { get; set; }
        
        /// <summary>
        /// Mensaje para el desarrollador que proporciona información adicional sobre el resultado de la operación.
        /// </summary> 
        public string TextoDesarrollador { get; set; }

        /// <summary>
        /// Constructor, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public RetornoBase()
        {
            Exito = false;
            TextoUsuario = string.Empty;
            TextoDesarrollador = string.Empty;
        }

        /// <summary>
        /// Constructor, inicializa atributos a vacío.
        /// </summary>
        /// <param name="param_exito">Indica si la operación fue exitosa</param>
        /// <param name="param_texto_usuario">Mensaje para el usuario</param>
        /// <param name="param_texto_desarrollador">Mensaje para el desarrollador</param>
        [JsonConstructor]
        public RetornoBase(bool param_exito, string param_texto_usuario, string param_texto_desarrollador = null)
        {
            Exito = param_exito;
            TextoUsuario = param_texto_usuario;
            TextoDesarrollador = param_texto_desarrollador;
        }
    }
}
