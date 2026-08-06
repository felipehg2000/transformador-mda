using System.Text.Json.Serialization;
using transformador_mda.src.Dominio.ElementosUML;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Objeto en el que almacenar toda la información relativa a la configuración del proceso de transformación.
    /// </summary>
    public class Configuracion
    {
        /// <summary>
        /// Tipo de diagrama a procesar, se utiliza para las factorías de generación de código.
        /// </summary>
        public EnumTipoDiagrama TipoDiagrama { get; set; }

        /// <summary>
        /// Tipo de fichero de entrada a procesar, se utiliza para las factorías de lectura de ficheros.
        /// </summary>
        public EnumTipoFicheroEntrada TipoFicheroEntrada { get; set; }

        /// <summary>
        /// Tipo de salida a generar, se utiliza para las factorías de generación de código.
        /// </summary>
        public EnumTipoSalida TipoSalida { get; set; }
        
        /// <summary>
        /// Información relativa al proyecto a generar y a los artefactos a crear y sus relaciones.
        /// </summary>
        public ProyectoBase Proyecto { get; set; }

        /// <summary>
        /// Ruta del fichero de entrada a procesar.
        /// </summary>
        public string RutaFicheroEntrada { get; set; }

        /// <summary>
        /// Ruta del directorio donde se generará la estructura leida del diagrama.
        /// </summary>
        public string RutaRaiz { get; set; }

        /// <summary>
        /// Ruta del fichero JSON del proyecto para la generación IA.
        /// </summary>
        public string RutaFicheroJSON { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public Configuracion()
        {
            TipoDiagrama = EnumTipoDiagrama.DiagramaDeClases;
            TipoFicheroEntrada = EnumTipoFicheroEntrada.XMI;
            TipoSalida = EnumTipoSalida.C_SHARP;
            Proyecto = new ProyectoBase();
            RutaFicheroEntrada = string.Empty;
            RutaRaiz = string.Empty;
            RutaFicheroJSON = string.Empty;
        }
    }
}
