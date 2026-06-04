using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using transformador_mda.src.Dominio;
using transformador_mda.src.Features.CrearIds;
using transformador_mda.src.Features.GenerarSalida;
using transformador_mda.src.Features.ParserEntrada;

namespace transformador_mda.src.Controladores
{
    /// <summary>
    /// Controlador principal, para procesar la lectura del XMI y la escritura de los artefactos
    /// </summary>
    public class Principal
    {
        #region "Propiedades"

        /// <summary>
        /// Atributo para almacenar toda la información necesaria para el generador.
        /// </summary>
        public Configuracion Configuracion { get; set; }

        #endregion

        #region "Constructor"

        public Principal()
        {

        }

        #endregion

        #region "Métodos públicos"

        /// <summary>
        /// Genera el parser correspondiente, llama a los métodos de lectura para rellenar los datos del Proyecto y rellena el resto de datos.
        /// </summary>
        /// <param name="param_nombre_proyecto">Nombre del proyecto a generar.</param>
        /// <param name="param_directorio_raiz">Ruta del directorio raíz del proyecto.</param>
        /// <returns>True si el proceso es correcto, false si hay algún fallo.</returns>
        public bool LeerFichero(string param_nombre_proyecto,
            string param_directorio_raiz)
        {
            IParserEntrada parser = FactoríaParser.CrearrParser(Configuracion);
            if (parser == null)
                return false;

            Configuracion.Proyecto = parser.Parsear();
            if (Configuracion.Proyecto == null)
                return false;

            Configuracion.Proyecto.Id = CrearIdentificador.CrearGUID();
            Configuracion.Proyecto.Nombre = param_nombre_proyecto;
            Configuracion.Proyecto.DirectorioRaiz = param_directorio_raiz; 

            return true;
        }

        /// <summary>
        /// Genera el generador correspondiente y llama al método de escritura.
        /// </summary>
        /// <returns>True si el proceso es correcto, false si no.</returns>
        public bool EscribirArtefactos()
        {
            GenerarSalidaBase generador = FactoriaSalida.CrearGenerarSalida(Configuracion);
            if (generador == null)
                return false;

            return generador.GenerarSalida();
        }

        #endregion

    }
}
