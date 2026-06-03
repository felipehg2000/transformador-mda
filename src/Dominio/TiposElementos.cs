namespace transformador_mda.src.Dominio
{
    /// <summary>
    /// Clase para gestionar los tipos de elementos a leer y generar.
    /// </summary>
    static public class TiposElementos
    {
        /// <summary>
        /// Enumeración para diferenciar los posibles tipos de elementos a generar.
        /// </summary>
        public enum Enumeracion
        {
            NULO = 0,           /// <summary> Elemento vacío, para inicializar atributos a vacío. </summary>
            ATRIBUTO = 1,       /// <summary> Representa un elemento de tipo atributo. </summary>
            PARAMETRO = 2,      /// <summary> Representa un elemento de tipo parámetro. </summary>
        }

        /// <summary>
        /// Según el texto de entrada, devuelve la enumeración correspondiente.
        /// </summary>
        /// <param name="param_valor">Texto de entrada que representa el tipo de elemento.</param>
        /// <returns>Enumeración correspondiente al texto de entrada.</returns>
        static public Enumeracion ObtenerEnumeracion(string param_valor)
        {
            if (string.IsNullOrWhiteSpace(param_valor))
                return Enumeracion.NULO;
            switch (param_valor.ToLower())
            {
                case "ownedAttribute":
                    return Enumeracion.ATRIBUTO;
                case "ownedParameter":
                    return Enumeracion.PARAMETRO;
                default:
                    return Enumeracion.NULO;
            }
        }

        /// <summary>
        /// Según la enumeración de entrada, devuelve el texto correspondiente para generar el código.
        /// </summary>
        /// <param name="param_enumeracion">Enumeración que representa el tipo de elemento.</param>
        /// <returns>Texto correspondiente a la enumeración para generar el código.</returns>
        static public string ObtenerValor(Enumeracion param_enumeracion)
        {
            switch (param_enumeracion)
            {
                case Enumeracion.ATRIBUTO:
                    return "ownedAttribute";
                case Enumeracion.PARAMETRO:
                    return "ownedParameter";
                default:
                    return string.Empty;
            }
        }
    }
}
