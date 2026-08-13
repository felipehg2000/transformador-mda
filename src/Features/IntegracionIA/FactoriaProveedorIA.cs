using System;

namespace transformador_mda.src.Features.IntegracionIA
{
    /// <summary>
    /// Enumeración con todos los clientes IA soportados por la aplicación.
    /// </summary>
    public enum ProveedorIA
    {
        OpenAI = 0 /// <summary> Identificador para el cliente Open AI </summary>
    }

    internal class FactoriaProveedorIA
    {
        /// <summary>
        /// Clase factoría. Según el proveedor de IA seleccionado por el usuairo genera y devuelve un proveedor de IA u otro.
        /// </summary>
        /// <param name="param_proveedor_ia">Cliente IA soportado por la aplicación</param>
        /// <param name="param_token">Token de conexión con el cliente IA.</param>
        /// <returns>Clase de conexión y generación de código de un proveedor IA.</returns>
        /// <exception cref="ArgumentException">Elemento de la enumeración no conocido.</exception>
        public static IProveedorIA CrearProveedorIA(ProveedorIA param_proveedor_ia, string param_token)
        {
            switch (param_proveedor_ia)
            {
                case ProveedorIA.OpenAI:
                    return new OpenAI(param_token);
                default:
                    throw new ArgumentException($"Proveedor de IA no soportado.");
            }
        }
    }
}
