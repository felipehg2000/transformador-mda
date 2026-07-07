namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Enumeración que representa los posibles tipos de elementos en UML.
    /// </summary>
    public enum EnumTipoElemento
    {
        NO_DEFINIDO = 0,
        CLASE = 1,
        ATRIBUTO = 2,
        METODO = 3,
        PARAMETRO = 4,
        ASOCIACION = 5
    }

    /// <summary>
    /// Enumeración que representa los posibles tipos de clases en UML.
    /// </summary>
    public enum EnumTiposClases
    {
        NO_DEFINIDA = 0,
        CLASE = 1,
        INTERFAZ = 2,
        CLASE_ABSTRACTA = 3,
        ENUMERACION = 4
    }

    /// <summary>
    /// Enumeración que representa los posibles tipos de asociaciones entre clases en UML.
    /// </summary>
    public enum EnumTipoAsociacion
    {
        NO_DEFINIDA = 0,
        ASOCIACION = 1,
        AGREGACION = 2,
        COMPOSICION = 3,
        HERENCIA = 4,
        DEPENDENCIA = 5
    }

    /// <summary>
    /// Enumeración que representa los posibles tipos de visibilidad de los elementos en UML.
    /// </summary>
    public enum EnumTipoVisibilidad
    {
        NO_DEFINIDA = 0,
        PUBLICO = 1,
        PRIVADO = 2,
        PROTEGIDO = 3,
        INTERNA = 4
    }
}
