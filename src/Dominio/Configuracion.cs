using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Enumeración, identifica el tipo de fichero de entrada a procesar.
    /// </summary>
    public enum EnumTipoFicheroEntrada
    {
        NULO = 0,   /// <summary> Inicialización a vacío, para iniciar los atributos a vacío. </summary>
        XMI = 1,    /// <summary> Único fichero de entrada soportado de momento, XML/XMI </summary>
    }

    /// <summary>
    /// Enumeración, identifica el tipo de diagrama a procesar.
    /// </summary>
    public enum EnumTipoDiagrama
    {
        NULO = 0,                   /// <summary> Inicialización a vacío, para iniciar los atributos a vacío. </summary>
        DiagramaDeClases = 1,       /// <summary> Único diagrama de entrada soportado de momento, diagrama de clases. </summary>
    }

    /// <summary>
    /// Enumeración, identifica el tipo de salida a generar.
    /// </summary>
    public enum EnumTipoSalida
    {
        NULO = 0,                   /// <summary> Inicialización a vacío, para iniciar los atributos a vacío. </summary>
        C_SHARP = 1,                /// <summary> Único tipo de salida soportado de momento, código C#. </summary>
    }

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
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public Configuracion()
        {
            TipoDiagrama = EnumTipoDiagrama.DiagramaDeClases;
            TipoFicheroEntrada = EnumTipoFicheroEntrada.NULO;
            TipoSalida = EnumTipoSalida.C_SHARP;
            Proyecto = new ProyectoBase();
            RutaFicheroEntrada = string.Empty;
        }
    }
}
