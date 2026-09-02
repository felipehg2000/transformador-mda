using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace transformador_mda.src.Dominio.ElementosUML
{
    /// <summary>
    /// Clase para almacenar la información sobre una operación.
    /// </summary>
    public class Metodo : ElementoVisible
    {
        /// <summary>
        /// Retorno del método.
        /// </summary>
        public Parametro Retorno { get; set; }

        /// <summary>
        /// Parámetros asociados al método.
        /// </summary>
        public List<Parametro> Parametros { get; set; }

        /// <summary>
        /// True si el método es un constructor, false si no.
        /// </summary>
        public bool EsConstructor { get; set; }

        /// <summary>
        /// True si el método es un destructor, false si no.
        /// </summary>
        public bool EsDestructor { get; set; }

        /// <summary>
        /// True si el método es abstracto, false si no.
        /// </summary>
        public bool EsAbstracto { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos.
        /// </summary>
        [JsonConstructor]
        public Metodo() : base()
        {
            Tipo = EnumTipoElemento.METODO;
            Retorno = new Parametro();
            Parametros = new List<Parametro>();
            EsConstructor = false;
            EsDestructor = false;
            EsAbstracto = false;
        }

        /// <summary>
        /// Busca un parámetro dentro de la lista de parámetros por su identificador.
        /// </summary>
        /// <param name="param_id">Identificador del parámetro a buscar.</param>
        /// <returns>El parámetro encontrado o null si no existe.</returns>
        public Parametro BuscarParametroPorId(string param_id)
        {
            return Parametros.Find(parametro => parametro.Id == param_id);
        }
    }
}
