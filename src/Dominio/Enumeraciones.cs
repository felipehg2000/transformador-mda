namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Enumeración, identifica el tipo de fichero de entrada a procesar.
    /// </summary>
    public enum EnumTipoFicheroEntrada
    {
        /// <summary> Inicialización a vacío, para iniciar los atributos a vacío. </summary>
        NULO = 0,

        /// <summary> Único fichero de entrada soportado de momento, XML/XMI </summary>
        XMI = 1,    
    }

    /// <summary>
    /// Enumeración, identifica el tipo de diagrama a procesar.
    /// </summary>
    public enum EnumTipoDiagrama
    {
        /// <summary> Inicialización a vacío, para iniciar los atributos a vacío. </summary>
        NULO = 0,

        /// <summary> Único diagrama de entrada soportado de momento, diagrama de clases. </summary>
        DiagramaDeClases = 1,       
    }

    /// <summary>
    /// Enumeración, identifica el tipo de salida a generar.
    /// </summary>
    public enum EnumTipoSalida
    {
        /// <summary> Inicialización a vacío, para iniciar los atributos a vacío. </summary>
        NULO = 0,

        /// <summary> Único tipo de salida soportado de momento, código C#. </summary>
        C_SHARP = 1,                
    }
}
