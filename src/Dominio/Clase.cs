using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Enumeración, identifica el tipo de clase a generar.
    /// </summary>
    public enum EnumTiposClase
    {
        NULO = 0,               /// <summary> Inicialización a vacío </summary>
        CLASE = 1,              /// <summary> Indica que el tipo de clase es una clase normal. </summary>
        INTERFAZ = 2,           /// <summary> Indica que el tipo de clase es una interfaz. </summary>
        CLASE_ABSTRACTA = 3,    /// <summary> Indica que el tipo de clase es una clase abstracta. </summary>
        ENUMERACION = 4,        /// <summary> Indica que el tipo de clase es una enumeración. </summary>
    }

    static public class TipoClase
    {
        static public EnumTiposClase ObtenerEnumeracion(string param_tipo_clase, bool param_es_abstracto)
        { 
            if (param_tipo_clase == "uml:Interface")
                return EnumTiposClase.INTERFAZ;
            else if (param_tipo_clase == "uml:Enumeration")
                return EnumTiposClase.ENUMERACION;
            else if (param_es_abstracto)
                return EnumTiposClase.CLASE_ABSTRACTA;
            else if (param_tipo_clase == "uml:Class")
                return EnumTiposClase.CLASE;

            return EnumTiposClase.NULO;
        }
    }



    /// <summary>
    /// Objeto en el que almacenar toda la información relativa a una clase (toda la información que ofrecel el documento XMI)
    /// </summary>
    public class Clase
    {
        /// <summary>
        /// Almacena el nombre de la clase.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Almacena el tipo de visibilidad de la clase
        /// </summary>
        public EnumVisibilidad Visibilidad { get; set; }

        /// <summary>
        /// Almacena el tipo de clase
        /// </summary>
        public EnumTiposClase TipoClase { get; set; }
        
        /// <summary>
        /// Almacena información sobre los atributos y propiedades
        /// </summary>
        public List<Elemento> Elementos { get; set; }

        /// <summary>
        /// Almacena información sobre los métodos
        /// </summary>
        public List<Metodo> Metodos { get; set; }

        /// <summary>
        /// Almacena información sobre las asociaciones de esta clase con otras.
        /// </summary>
        public List<Asociacion> Asociaciones { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public Clase()
        {
            Nombre = string.Empty;
            Visibilidad = EnumVisibilidad.NULO;
            TipoClase = EnumTiposClase.NULO;
            Elementos = new List<Elemento>();
            Metodos = new List<Metodo>();
            Asociaciones = new List<Asociacion>();
        }
    }
}
