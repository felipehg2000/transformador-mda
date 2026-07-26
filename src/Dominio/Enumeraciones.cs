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
}
