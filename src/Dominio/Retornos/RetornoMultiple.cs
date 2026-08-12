using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.Retornos
{
    /// <summary>
    /// Retorno con un valor de estado para definir diferentes situaciones según el proceso lógico llevado a cabo.
    /// </summary>
    public class RetornoMultiple : RetornoBase
    {
        /// <summary>
        /// Enumeración con posibles estados de un algoritmo.
        /// </summary>
        public enum EnumEstado
        {
            NO_DEFINIDO = 0,                /// <summary>Estado no definido.</summary>
            CORRECTO = 1,                   /// <summary>Estado correcto.</summary>
            ERROR = 2,                      /// <summary>Estado incorrecto.</summary>
            CANCELADO_POR_USUARIO = 3,      /// <summary>Proceso cancelado por el usuario.</summary>
            CONFIRMACION_USUARIO = 4        /// <summary>El proceso puede continuar, pero a falta de confirmación por el usuario porque faltan requisitos no fundamentales.</summary>
        }

        /// <summary>
        /// Estado del proceso lógico que se está ejecutando al finalizar este proceso.
        /// </summary>
        public EnumEstado Estado { get; set; }

        /// <summary>
        /// Constructor, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public RetornoMultiple()
        {
            Estado = EnumEstado.NO_DEFINIDO;
        }

        /// <summary>
        /// Constructor, inicializa atributos.
        /// </summary>
        /// <param name="param_exito">True si el proceso es correcto, false si no.</param>
        /// <param name="param_estado">Situación de fin del proceso lógico.</param>
        /// <param name="param_texto_usuario">Texto a mostrar al usuario, sin información relevante del proceso lógico.</param>
        /// <param name="param_texto_desarrollador">Texto a disposición del desarrollador, para devolver información o datos.</param>
        public RetornoMultiple(bool param_exito,
            EnumEstado param_estado,
            string param_texto_usuario,
            string param_texto_desarrollador = null) :
                base(param_exito,
                    param_texto_usuario,
                    param_texto_desarrollador)
        {
            Estado = param_estado;
        }

    }
}
