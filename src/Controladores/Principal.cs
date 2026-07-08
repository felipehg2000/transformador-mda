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
        /// Genera el parser correspondiente, llama a los métodos de lectura para rellenar los datos del Proyecto y rellena el resto de datos.
        /// </summary>
        /// <param name="param_nombre_proyecto">Nombre del proyecto a generar.</param>
        /// <param name="param_directorio_raiz">Ruta del directorio raíz del proyecto.</param>
        /// <returns>True si el proceso es correcto, false si hay algún fallo.</returns>
        public bool LeerFichero(string param_ruta_fichero)
        {
            Configuracion.RutaFicheroEntrada = param_ruta_fichero;

            ParserUMLBase parser = FactoríaParser.CrearParser(Configuracion);
            if (parser == null)
                return false;

            RetornoElementoUML ret_resultado = parser.Parsear();
            Configuracion.Proyecto = ret_resultado.Elemento as ProyectoBase;
            if (Configuracion.Proyecto == null)
                return false;

            Configuracion.Proyecto.Id = CrearIdentificador.CrearGUID();

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

    }
}
