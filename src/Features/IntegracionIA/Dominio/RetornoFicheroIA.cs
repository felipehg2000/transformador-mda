using transformador_mda.src.Dominio.Retornos;

namespace transformador_mda.src.Features.IntegracionIA.Dominio
{
    /// <summary>
    /// Objeto de retorno, con información del proceso y, en caso de que todo sea correcto información del fichero generado y subido a la IA.
    /// </summary>
    internal class RetornoFicheroIA : RetornoBase
    {
        /// <summary>
        /// Información del fichero subido a la IA.
        /// </summary>
        internal InformacionFichero InformacionFichero { get; set; }

        /// <summary>
        /// Constructor, inicializa atributos a vacío.
        /// </summary>
        public RetornoFicheroIA() : base()
        {
            InformacionFichero = new InformacionFichero();
        }

        /// <summary>
        /// Constructor, inicializa atributos.
        /// </summary>
        /// <param name="param_exito">Indica si la operación fue exitosa (true) o no (false).</param>
        /// <param name="param_mensaje_usuario">Mensaje para el usuario</param>
        /// <param name="param_mensaje_desarrollador">Mensaje para el desarrollador</param>
        /// <param name="param_informacion_fichero">Información del fichero subido a la IA.</param>
        public RetornoFicheroIA(bool param_exito,
            string param_mensaje_usuario,
            string param_mensaje_desarrollador,
            InformacionFichero param_informacion_fichero) : base(param_exito,
                param_mensaje_usuario,
                param_mensaje_desarrollador)
        {
            InformacionFichero = param_informacion_fichero;
        }
    }
}
