using System;

namespace transformador_mda.src.Features.ParserEntrada
{
    /// <summary>
    /// Clase que define las claves soportadas en el proceso de lectura del ficheros XMI.
    /// </summary>
    static internal class ClavesSoportadasXMI
    {
        /// <summary>
        /// Enumeración que define las claves soportadas en el proceso de lectura del ficheros XMI.
        /// </summary>
        internal enum Enumeracion
        {
            NULO = 0,
            CLASE = 1,
            ASOCIACION = 2,
            PAQUETE = 3,
            ATRIBUTO = 4,
            METODO = 5,
            PARAMETRO = 6,
            HERENCIA = 7,
        }

        /// <summary>
        /// Método que devuelve la clave asociada a un valor de la enumeración.
        /// </summary>
        /// <param name="param_enum_clave">Valor de la enumeración para el cual se desea obtener la clave.</param>
        /// <returns>Clave asociada al valor de la enumeración.</returns>
        /// <exception cref="ArgumentException">Se lanza si el valor de la enumeración no está soportado.</exception>
        static internal string ObtenerClave(Enumeracion param_enum_clave)
        {
            switch (param_enum_clave)
            {
                case Enumeracion.CLASE:
                    return "packagedElement xmi:type=\"uml:Class\"";
                case Enumeracion.ASOCIACION:
                    return "packagedElement xmi:type=\"uml:Association\"";
                case Enumeracion.PAQUETE:
                    return "packagedElement xmi:type=\"uml:Package\"";
                case Enumeracion.ATRIBUTO:
                    return "ownedAttribute";
                case Enumeracion.METODO:
                    return "ownedOperation";
                case Enumeracion.PARAMETRO:
                    return "ownedParameter";
                case Enumeracion.HERENCIA:
                    return "generalization";
                default:
                    throw new ArgumentException("Valor de enum no soportado");
            }
        }

        /// <summary>
        /// Método que devuelve el valor de la enumeración asociado a una clave.
        /// </summary>
        /// <param name="param_clave">Clave para la cual se desea obtener el valor de la enumeración.</param>
        /// <returns>Valor de la enumeración asociado a la clave.</returns>
        static public Enumeracion ObtenerEnumClave(string param_clave)
        {
            if (param_clave == "packagedElement xmi:type=\"uml:Class\"")
                return Enumeracion.CLASE;
            else if (param_clave == "packagedElement xmi:type=\"uml:Association\"")
                return Enumeracion.ASOCIACION;
            else if (param_clave == "packagedElement xmi:type=\"uml:Package\"")
                return Enumeracion.PAQUETE;
            else if (param_clave == "ownedAttribute")
                return Enumeracion.ATRIBUTO;
            else if (param_clave == "ownedOperation")
                return Enumeracion.METODO;
            else if (param_clave == "ownedParameter")
                return Enumeracion.PARAMETRO;
            else if (param_clave == "generalization")
                return Enumeracion.HERENCIA;
            return Enumeracion.NULO;
        }
    }
}
