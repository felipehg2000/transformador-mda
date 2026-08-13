using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Features.IntegracionIA.Dominio
{
    /// <summary>
    /// Almacena toda la información de todos los ficheros que se han subido a OpenAI.
    /// </summary>
    internal class InformacionIA
    {
        /// <summary>
        /// Directorio donde se almacenan todos los ficheros generados que se van a subir a OpenAI.
        /// </summary>
        internal string RutaDirectorioFicheros { get; set; }

        /// <summary>
        /// Información del fichero JSON generado y subido a OpenAI. 
        /// </summary>
        /// <remarks>
        /// Este fichero tiene toda la información del proyecto.
        /// </remarks>
        internal InformacionFichero InformacionJSON { get; set; }

        /// <summary>
        /// Información de los ficheros de código esqueleto que se han generado para subirse a OpenAI.
        /// </summary>
        internal List<InformacionFichero> InformacionFicherosEsqueleto { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public InformacionIA()
        {
            RutaDirectorioFicheros = string.Empty;
            InformacionJSON = new InformacionFichero();
            InformacionFicherosEsqueleto = new List<InformacionFichero>();
        }
    }
}
