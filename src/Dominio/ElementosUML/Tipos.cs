namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Enumeración que representa los posibles tipos de elementos en UML.
    /// </summary>
    public enum EnumTipoElemento
    {
        NO_DEFINIDO = 0,    /// <summary> Tipo de elemento no definido </summary>    
        PROYECTO = 1,       /// <summary> Tipo de elemento proyecto </summary>
        DIRECTORIO = 2,     /// <summary> Tipo de elemento directorio </summary>
        CLASE = 3,          /// <summary> Tipo de elemento clase </summary>
        ATRIBUTO = 4,       /// <summary> Tipo de elemento atributo </summary>
        METODO = 5,         /// <summary> Tipo de elemento método </summary>
        PARAMETRO = 6,      /// <summary> Tipo de elemento parámetro </summary>
        ASOCIACION = 7      /// <summary> Tipo de elemento asociación </summary>
    }

    /// <summary>
    /// Enumeración que representa los posibles tipos de clases en UML.
    /// </summary>
    public enum EnumTiposClases
    {
        NO_DEFINIDA = 0,        /// <summary> Tipo de clase no definida </summary>
        CLASE = 1,              /// <summary> Tipo de clase </summary>
        INTERFAZ = 2,           /// <summary> Tipo de interfaz </summary>
        CLASE_ABSTRACTA = 3,    /// <summary> Tipo de clase abstracta </summary>
        ENUMERACION = 4         /// <summary> Tipo de enumeración </summary>
    }

    /// <summary>
    /// Enumeración que representa los posibles tipos de asociaciones entre clases en UML.
    /// </summary>
    public enum EnumTipoAsociacion
    {
        NO_DEFINIDA = 0,    /// <summary> Tipo de asociación no definida </summary>
        ASOCIACION = 1,     /// <summary> Tipo de asociación </summary>
        AGREGACION = 2,     /// <summary> Tipo de agregación </summary>
        COMPOSICION = 3,    /// <summary> Tipo de composición </summary>
        HERENCIA = 4,       /// <summary> Tipo de herencia </summary>
        DEPENDENCIA = 5     /// <summary> Tipo de dependencia </summary>
    }

    /// <summary>
    /// Enumeración que representa los posibles tipos de visibilidad de los elementos en UML.
    /// </summary>
    public enum EnumTipoVisibilidad
    {
        NO_DEFINIDA = 0,    /// <summary> Tipo de visibilidad no definida </summary>
        PUBLICO = 1,        /// <summary> Tipo de visibilidad pública </summary>
        PRIVADO = 2,        /// <summary> Tipo de visibilidad privada </summary>
        PROTEGIDO = 3,      /// <summary> Tipo de visibilidad protegida </summary>
        INTERNA = 4         /// <summary> Tipo de visibilidad interna </summary>
    }
}
