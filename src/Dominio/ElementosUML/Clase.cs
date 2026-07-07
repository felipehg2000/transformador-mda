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
        /// Listado con los atributos y parámetros de la clase.
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
        public Clase() : base()
        {
            Elementos = new List<Elemento>();
            Metodos = new List<Metodo>();
            Asociaciones = new List<Asociacion>();
        }

        /// <summary>
        /// Método para obtener un elemento de la clase a través de su identificador único.
        /// </summary>
        /// <param name="param_id">Identificador del elemento a buscar.</param>
        /// <returns>El elemento encontrado o null si no existe.</returns>
        public Elemento ObtenerElementoPorId(string param_id)
        {
            return Elementos.Find(elemento => elemento.Id == param_id);
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
    }
}
