namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Enumeración que representa los posibles tipos de elementos en UML.
    /// </summary>
    public enum EnumTipoElemento
    {
        /// <summary> Tipo de elemento no definido </summary>    
        NO_DEFINIDO = 0,
        
        /// <summary> Tipo de elemento proyecto </summary>
        PROYECTO = 1,

        /// <summary> Tipo de elemento directorio </summary>
        DIRECTORIO = 2,

        /// <summary> Tipo de elemento clase </summary>
        CLASE = 3,

        /// <summary> Tipo de elemento atributo </summary>
        ATRIBUTO = 4,

        /// <summary> Tipo de elemento método </summary>
        METODO = 5,

        /// <summary> Tipo de elemento parámetro </summary>
        PARAMETRO = 6,

        /// <summary> Tipo de elemento parámetro de tipo retorno de un método. </summary>
        RETORNO = 7,

        /// <summary> Tipo de elemento literal (para enumeraciones) </summary>
        LITERAL = 8,

        /// <summary> Tipo de elemento asociación </summary>
        ASOCIACION = 9      
    }

    /// <summary>
    /// Enumeración que representa los posibles tipos de clases en UML.
    /// </summary>
    public enum EnumTiposClases
    {
        /// <summary> Tipo de clase no definida </summary>
        NO_DEFINIDA = 0,

        /// <summary> Tipo de clase </summary>
        CLASE = 1,

        /// <summary> Tipo de interfaz </summary>
        INTERFAZ = 2,

        /// <summary> Tipo de clase abstracta </summary>
        CLASE_ABSTRACTA = 3,

        /// <summary> Tipo de enumeración </summary>
        ENUMERACION = 4         
    }

    /// <summary>
    /// Enumeración que representa los posibles tipos de asociaciones entre clases en UML.
    /// </summary>
    public enum EnumTipoAsociacion
    {
        /// <summary> Tipo de asociación no definida </summary>
        NO_DEFINIDA = 0,

        /// <summary> Tipo de asociación </summary>
        ASOCIACION = 1,

        /// <summary> Tipo de agregación </summary>
        AGREGACION = 2,

        /// <summary> Tipo de composición </summary>
        COMPOSICION = 3,

        /// <summary> Tipo de herencia </summary>
        HERENCIA = 4,

        /// <summary> Tipo de dependencia </summary>
        DEPENDENCIA = 5     
    }

    /// <summary>
    /// Enumeración que representa los posibles tipos de visibilidad de los elementos en UML.
    /// </summary>
    public enum EnumTipoVisibilidad
    {
        /// <summary> Tipo de visibilidad no definida </summary>
        NO_DEFINIDA = 0,

        /// <summary> Tipo de visibilidad pública </summary>
        PUBLICO = 1,

        /// <summary> Tipo de visibilidad privada </summary>
        PRIVADO = 2,

        /// <summary> Tipo de visibilidad protegida </summary>
        PROTEGIDO = 3,

        /// <summary> Tipo de visibilidad interna </summary>
        INTERNA = 4         
    }

    /// <summary>
    /// Enumeración que representa un tipo de dato.
    /// </summary>
    public enum EnumTipoDato
    {
        /// <summary> Tipo de dato no definido </summary>
        NO_DEFINIDO = 0,

        /// <summary> Tipo de dato entero </summary>
        ENTERO = 1,

        /// <summary> Tipo de dato flotante </summary>
        FLOTANTE = 2,

        /// <summary> Tipo de dato doble </summary>
        DOBLE = 3,

        /// <summary> Tipo de dato booleano </summary>
        BOOLEANO = 4,

        /// <summary> Tipo de dato carácter </summary>
        CARACTER = 5,

        /// <summary> Tipo de dato cadena </summary>
        CADENA = 6,         

        /// <summary> Tipo de dato entero largo </summary>
        LARGO = 7,

        /// <summary> Tipo de dato entero corto </summary>
        CORTO = 8,

        /// <summary> Tipo de dato Blob </summary>
        BYTE = 9
    }
}
