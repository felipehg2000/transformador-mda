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
        /// Tipo de dato que retorna el método (bool, int, void...)
        /// </summary>
        public EnumTipoDato TipoRetorno { get; set; }

        /// <summary>
        /// Parámetros asociados al método.
        /// </summary>
        public List<Parametro> Parametros { get; set; }

        /// <summary>
        /// Constructor de la clase, inicializa atributos.
        /// </summary>
        [JsonConstructor]
        public Metodo() : base()
        {
            Tipo = EnumTipoElemento.METODO;
            TipoRetorno = EnumTipoDato.NO_DEFINIDO;
            Parametros = new List<Parametro>();
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
