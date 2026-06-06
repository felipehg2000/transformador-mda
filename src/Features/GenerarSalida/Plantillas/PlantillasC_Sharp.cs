namespace transformador_mda.src.Features.GenerarSalida.Plantillas
{
    /// <summary>
    /// Clase para almacenar las plantillas de CSharp.
    /// </summary>
    public class PlantillasC_Sharp
    {
        /// <summary>
        /// Plantilla para las clases básicas.
        /// </summary>
        public static string ClaseBase =
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
{Relaciones}

namespace {Namespace}
{
    {Visibilidad} {TipoClase} {NombreClase} {Herencia}
    {
        #region ""Atributos""

{Atributos}

        #endregion

        #region ""Propiedades""

{Propiedades}

        #endregion

        #region ""Métodos Públicos""

{MetodosPublicos}

        #endregion  

        #region ""Métodos Protegidos""

{MetodosProtegidos}

        #endregion

        #region ""Métodos Privados""

{MetodosPrivados}

        #endregion
    }
}";

        /// <summary>
        /// Plantilla para los métodos base.
        /// </summary>
        public static string Metodo =
@"
        {Visibilidad} {TipoRetorno} {NombreMetodo}({Parametros})
        {

        }";
    }
}
