using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Objeto en el que almacenar toda la información relativa a una clase (toda la información que ofrece el documento XMI)
    /// </summary>
    public class Clase : ElementoVisible
    {
        /// <summary>
        /// Tipo de clase (clase, interfaz, enumeración, etc.)
        /// </summary>
        public EnumTiposClases TipoClase { get; set; }

        /// <summary>
        /// Listado con los atributos y parámetros de la clase.
        /// </summary>
        public List<Atributo> Atributos { get; set; }

        /// <summary>
        /// Almacena información sobre los métodos
        /// </summary>
        public List<Metodo> Metodos { get; set; }

        /// <summary>
        /// Almacena información sobre los literales de las clases (si es una enumeración)
        /// </summary>
        public List<ElementoBase> Literales { get; set; }

        /// <summary>
        /// Almacena información sobre las asociaciones de esta clase con otras.
        /// </summary>
        public List<Asociacion> Asociaciones { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>
        [JsonConstructor]
        public Clase() : base()
        {
            Tipo = EnumTipoElemento.CLASE;
            TipoClase = EnumTiposClases.NO_DEFINIDA;
            Atributos = new List<Atributo>();
            Metodos = new List<Metodo>();
            Asociaciones = new List<Asociacion>();
            Literales = new List<ElementoBase>();
        }

        /// <summary>
        /// Método para obtener un atributo de la clase a través de su identificador único.
        /// </summary>
        /// <param name="param_id">Identificador del atributo a buscar.</param>
        /// <returns>El atributo encontrado o null si no existe.</returns>
        public Atributo ObtenerAtributoPorId(string param_id)
        {
            return Atributos.Find(atributo => atributo.Id == param_id);
        }

        /// <summary>
        /// Método para obtener un método de la clase a través de su identificador único.
        /// </summary>
        /// <param name="param_id">Identificador del método a buscar.</param>
        /// <returns>El método encontrado o null si no existe.</returns>
        public Metodo ObtenerMetodoPorId(string param_id)
        {
            return Metodos.Find(metodo => metodo.Id == param_id);
        }

        /// <summary>
        /// Método para obtener una asociación de la clase a través de su identificador único.
        /// </summary>
        /// <param name="param_id">Identificador de la asociación a buscar.</param>
        /// <returns>La asociación encontrada o null si no existe.</returns>
        public Asociacion ObtenerAsociacionPorId(string param_id)
        {
            return Asociaciones.Find(asociacion => asociacion.Id == param_id);
        }

        /// <summary>
        /// Método para obtener un literal de una enumeración a través de su identificador único.
        /// </summary>
        /// <param name="param_id">Identificador del literal a buscar.</param>
        /// <returns>El literal encontrado o null si no existe.</returns>
        public ElementoBase ObtenerLiteralPorId(string param_id)
        {
            return Literales.Find(literal => literal.Id == param_id);
        }
    }
}
