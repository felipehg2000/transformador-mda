using System;

namespace transformador_mda.src.Features.IntegracionIA
{
    public enum ProveedorIA
    {
        OpenAI
    }

    internal class FactoriaProveedorIA
    {
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
