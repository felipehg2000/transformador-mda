namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Enumeración para indicar los diferentes tipos de visibilidad de los artefactos software a generar.
    /// </summary>
    public enum EnumVisibilidad
    {
        NULO = 0,           /// <summary> Valor para inicializar a vacío los atributos. </summary>
        PUBLICO = 1,        /// <summary> Indica una visibilidad de tipo public para clases, métodos, propiedades y atributos.</summary>
        PRIVADO = 2,        /// <summary> Indica una visibilidad de tipo private para clases, métodos, propiedades y atributos.</summary>
        PROTEGIDO = 3,      /// <summary> Indica una visibilidad de tipo protected para clases, métodos, propiedades y atributos.</summary>
        INTERNA = 4,        /// <summary> Indica una visibilidad de tipo internal para clases. </summary>
    }
}
