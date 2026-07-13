using System;
using transformador_mda.src.Dominio;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Features.CrearIds;
using transformador_mda.src.Features.ExploradorFicheros;
using transformador_mda.src.Features.GenerarSalida;
using transformador_mda.src.Features.GestionConfiguracion;
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

        /// <summary>
        /// Constructor de la clase, inicializa atributos.
        /// </summary>
        public Principal()
        {
            Configuracion = new Configuracion();
        }

        #endregion

        #region "Métodos públicos"

        /// <summary>
        /// Permite seleccionar el fichero XMI y realiza el proceso de lectura.
        /// </summary>
        /// <returns>Objeto con información del proceso.</returns>
        public RetornoMultiple BuscarFichero()
        {
            string extensiones_soportadas = string.Empty;
            if (Configuracion.TipoFicheroEntrada == EnumTipoFicheroEntrada.NULO)
                return new RetornoMultiple(false, 
                    RetornoMultiple.EnumEstado.ERROR,
                    "No se ha podido leer el fichero correctamente.",
                    "El tipo de dato que define el formato del fichero de entrada no está definido.");
            else if (Configuracion.TipoFicheroEntrada == EnumTipoFicheroEntrada.XMI)
                extensiones_soportadas = "Archivos XMI (*.xmi) | *.xmi| Archivos XML (*.xml) | *.xml| Todos los archivos (*.*) | *.*";
            
            string ruta_fichero = Files.GetFilePath("Buscar fichero de entrada.", extensiones_soportadas);
            if (string.IsNullOrEmpty(ruta_fichero))
                return new RetornoMultiple(true,
                    RetornoMultiple.EnumEstado.CANCELADO_POR_USUARIO,
                    "Proceso cancelado por el usuario.");

            RetornoMultiple ret_retorno = (RetornoMultiple)LeerFichero(ruta_fichero);
            if (!ret_retorno.Exito)
            {
                ret_retorno.Estado = RetornoMultiple.EnumEstado.ERROR;
                return ret_retorno;
            }

            Configuracion.RutaFicheroEntrada = ruta_fichero;

            ret_retorno.Estado = RetornoMultiple.EnumEstado.CORRECTO;
            return ret_retorno;
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

        /// <summary>
        /// Método para llamar a la funcionalidad de exportar configuración.
        /// </summary>
        /// <returns>True si el proceso ha sido correcto, false si el usuario ha cancelado el proceso.</returns>
        public bool ExportarConfiguracion()
        {
            string ruta_directorio = Folder.GetFolderPath("Directorio para almacenar la configuración...");
            if (string.IsNullOrEmpty(ruta_directorio))
                return false;

            DAO.Exportar(Configuracion, ruta_directorio);
            return true;
        }

        /// <summary>
        /// Método para llamar a la funcionalidad de importar configuración.
        /// </summary>
        /// <returns>False si el usuario ha cancelado el proceso true si ha ido todo bien.</returns>
        public bool ImportarConfiguracion()
        {
            string nombre_fichero = Files.GetFilePath("Buscar fichero XMI.",
                "Archivos JSON (*.json) | *.json| Todos los archivos (*.*) | *.*");
            if (string.IsNullOrEmpty(nombre_fichero))
                return false;

            Configuracion = DAO.Importar(nombre_fichero);
            return true;
        }

        #endregion

        #region "Métodos privados"

        /// <summary>
        /// Genera el parser según el tipo de formato de entrada y carga el objeto con los datos del fichero.
        /// </summary>
        /// <param name="param_ruta_fichero">Ruta del fichero XMI</param>
        /// <returns>Objeto de tipo RetornoBase con información del proceso.</returns>
        private RetornoBase LeerFichero(string param_ruta_fichero)
        {
            Configuracion.RutaFicheroEntrada = param_ruta_fichero;

            ParserUMLBase parser = FactoríaParser.CrearParser(Configuracion);
            if (parser == null)
                return new RetornoBase(false,
                    "No se ha podido leer el fichero correctamente.",
                    "No se ha creado el parser correctamente.");

            RetornoElementoUML ret_resultado = parser.Parsear();
            if (!ret_resultado.Exito)
                return ret_resultado;

            Configuracion.Proyecto = ret_resultado.Elemento as ProyectoBase;
            if (Configuracion.Proyecto == null)
                return new RetornoBase(false,
                    "No se ha podido leer el fichero correctamente.",
                    "El proyecto está vacío, la lectura ha sido correcta pero no se han leido datos y el objeto Proyecto está vacío.");

            Configuracion.Proyecto.Id = CrearIdentificador.CrearGUID();

            return new RetornoBase(true, "El fichero se ha leido correctamente.");
        }

        #endregion

    }
}
