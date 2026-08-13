using System.Text.Json.Serialization;

namespace transformador_mda.src.Features.IntegracionIA.Dominio
{
    /// <summary>
    /// Objeto para persistir la información sobre un fichero generado y subido a open AI.
    /// </summary>
    internal class InformacionFichero
    {
        /// <summary>
        /// Identificador de una clase cuyo esqueleto se ha almacenado en un fichero.
        /// </summary>
        internal string IdClaseDelEsqueleto { get; set; }

        /// <summary>
        /// Ruta donde se ha almacenado el esqueleto de una clase
        /// </summary>
        internal string RutaFichero { get; set; }

        /// <summary>
        /// Identificador del fichero en OpenAI.
        /// </summary>
        internal string IdFicheroOpenAI { get; set; }

        /// <summary>
        /// Constructor, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public InformacionFichero()
        {
            IdClaseDelEsqueleto = string.Empty;
            RutaFichero = string.Empty;
            IdFicheroOpenAI = string.Empty;
        }

        /// <summary>
        /// Constructor de copia, inicializa atributos.
        /// </summary>
        /// <param name="param_ruta_fichero">Identificador de una clase cuyo esqueleto se ha almacenado en un fichero.</param>
        /// <param name="param_id_fichero_openai">Identificador del fichero en OpenAI.</param>
        /// <param name="param_id_clase_esqueleto">Identificador del fichero en OpenAI.</param>
        public InformacionFichero(string param_ruta_fichero, string param_id_fichero_openai, string param_id_clase_esqueleto = null)
        {
            RutaFichero = param_ruta_fichero;
            IdFicheroOpenAI = param_id_fichero_openai;
            IdClaseDelEsqueleto = param_id_clase_esqueleto ?? string.Empty;
        }
    }
}
