using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using transformador_mda.src.Dominio;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Features.DAO;
using transformador_mda.src.Features.GenerarSalida;
using transformador_mda.src.Features.IntegracionIA;
using transformador_mda.src.Features.ParserEntrada;
using transformador_mda.src.Features.Utilidades;

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
            Configuracion = new Dominio.Configuracion();
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

            string ruta_fichero = Ficheros.GetFilePath("Buscar fichero de entrada.", extensiones_soportadas);
            if (string.IsNullOrEmpty(ruta_fichero))
                return new RetornoMultiple(true,
                    RetornoMultiple.EnumEstado.CANCELADO_POR_USUARIO,
                    "Proceso cancelado por el usuario.");

            RetornoBase retorno = LeerFichero(ruta_fichero);
            RetornoMultiple ret_retorno = new RetornoMultiple()
            {
                Exito = retorno.Exito,
                TextoUsuario = retorno.TextoUsuario,
                TextoDesarrollador = retorno.TextoDesarrollador,
                Estado = retorno.Exito ? RetornoMultiple.EnumEstado.CORRECTO : RetornoMultiple.EnumEstado.ERROR
            };

            if (ret_retorno.Exito)
                Configuracion.RutaFicheroEntrada = ruta_fichero;

            return ret_retorno;
        }

        /// <summary>
        /// Muestra el selector de directorios y, si el proyecto no está nombrado coge el nombre del directorio como nombre del proyecto.
        /// </summary>
        /// <returns>Objeto con información del proceso.</returns>
        public RetornoMultiple BuscarDirectorioRaiz()
        {
            string ruta_directorio = Folder.GetFolderPath("Seleccione el directorio raíz del proyecto...");
            if (string.IsNullOrEmpty(ruta_directorio))
                return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CANCELADO_POR_USUARIO, "Operación cancelada por el usuario.");

            Configuracion.RutaRaiz = ruta_directorio;

            if (string.IsNullOrEmpty(Configuracion.Proyecto.Nombre))
                Configuracion.Proyecto.Nombre = Path.GetFileName(ruta_directorio);

            return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CORRECTO, "Directorio seleccionado correctamente.");
        }

        public RetornoBase GenerarCodigo(Clase param_proyecto)
        {
            GenerarSalidaBase generador = FactoriaSalida.CrearGenerarSalida(Configuracion);
            string texto = generador.GenerarTexto(param_proyecto).TextoDesarrollador;

            return new RetornoBase(true, "Generación correcta", texto);
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

            ConfiguracionDAO.Exportar(Configuracion, ruta_directorio);
            return true;
        }

        /// <summary>
        /// Método para llamar a la funcionalidad de importar configuración.
        /// </summary>
        /// <returns>False si el usuario ha cancelado el proceso true si ha ido todo bien.</returns>
        public bool ImportarConfiguracion()
        {
            string nombre_fichero = Ficheros.GetFilePath("Buscar fichero XMI.",
                "Archivos JSON (*.json) | *.json| Todos los archivos (*.*) | *.*");
            if (string.IsNullOrEmpty(nombre_fichero))
                return false;

            Configuracion = ConfiguracionDAO.Importar(nombre_fichero);
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

        #region "Actualización de datos"

        /// <summary>
        /// Método encargado de actualizar los comentarios de los componentes de tipo parámetro.
        /// </summary>
        /// <param name="param_id">Identificador del elemento UML.</param>
        /// <param name="param_descripcion">Comentario con el que se actualizará el componente.</param>
        /// <returns>True si el proceso es correcto, false si no.</returns>
        public bool ActualizarParametro(string param_id, string param_descripcion)
        {
            Parametro parametro = BuscarComponentes.BuscarParametro(Configuracion.Proyecto, param_id);
            if (parametro == null)
                return false;

            parametro.Comentario = param_descripcion;
            return true;
        }

        /// <summary>
        /// Método encargado de actualizar los comentarios de los componentes de tipo clase.
        /// </summary>
        /// <param name="param_id">Identificador del elemento UML.</param>
        /// <param name="param_descripcion">Comentario con el que se actualizará el componente.</param>
        /// <returns>True si el proceso es correcto, false si no.</returns>
        public bool ActualizarClase(string param_id, string param_descripcion)
        {
            Clase clase = BuscarComponentes.BuscarClase(Configuracion.Proyecto, param_id);
            if (clase == null)
                return false;

            clase.Comentario = param_descripcion;
            return true;
        }

        /// <summary>
        /// Método encargado de actualizar los comentarios de los componentes de tipo atributo.
        /// </summary>
        /// <param name="param_id">Identificador del elemento UML.</param>
        /// <param name="param_descripcion">Comentario con el que se actualizará el componente.</param>
        /// <returns>True si el proceso es correcto, false si no.</returns>
        public bool ActualizarAtributo(string param_id, string param_descripcion)
        {
            Atributo atributo = BuscarComponentes.BuscarAtributo(Configuracion.Proyecto, param_id);
            if (atributo == null)
                return false;

            atributo.Comentario = param_descripcion;
            return true;
        }

        /// <summary>
        /// Método encargado de actualizar los comentarios de los componentes de tipo asociación.
        /// </summary>
        /// <param name="param_id">Identificador del elemento UML.</param>
        /// <param name="param_descripcion">Comentario con el que se actualizará el componente.</param>
        /// <returns>True si el proceso es correcto, false si no.</returns>
        public bool ActualizarAsociaciones(string param_id, string param_descripcion)
        {
            Asociacion asociacion = BuscarComponentes.BuscarAsociacion(Configuracion.Proyecto, param_id);
            if (asociacion == null)
                return false;

            asociacion.Comentario = param_descripcion;
            return true;
        }

        /// <summary>
        /// Método encargado de actualizar los comentarios de los componentes de tipo método.
        /// </summary>
        /// <param name="param_id">Identificador del elemento UML.</param>
        /// <param name="param_descripcion">Comentario con el que se actualizará el componente.</param>
        /// <returns>True si el proceso es correcto, false si no.</returns>
        public bool ActualizarMetodos(string param_id, string param_descripcion, string param_comentario_retorno)
        {
            Metodo metodo = BuscarComponentes.BuscarMetodo(Configuracion.Proyecto, param_id);
            if (metodo == null)
                return false;

            metodo.Comentario = param_descripcion;
            metodo.Retorno.Comentario = param_comentario_retorno;
            return true;
        }

        /// <summary>
        /// Método encargado de actualizar los comentarios de los componentes de tipo literal.
        /// </summary>
        /// <param name="param_id">Identificador del elemento UML.</param>
        /// <param name="param_descripcion">Comentario con el que se actualizará el componente.</param>
        /// <returns>True si el proceso es correcto, false si no.</returns>
        public bool ActualizarLiteral(string param_id, string param_descripcion)
        {
            ElementoBase literal = BuscarComponentes.BuscarLiteral(Configuracion.Proyecto, param_id);
            if (literal == null)
                return false;

            literal.Comentario = param_descripcion;
            return true;
        }

        #endregion

        public async Task<RetornoBase> GenerarCodigoProyectoCompleto()
        {
            throw new NotImplementedException();
        }

        public async Task<RetornoBase> GenerarCodigoClase(Clase param_clase, bool param_generar_json)
        {
            if (param_generar_json && string.IsNullOrEmpty(Configuracion.RutaFicheroJSON))
            {
                if (Configuracion.Proyecto == null)
                    return new RetornoBase(false, "No se ha podido generar el código de la clase.", "El proyecto está vacío.");

                RetornoBase retorno_exportacion = ProyectoDAO.Exportar(Configuracion.Proyecto, Application.StartupPath);
                if (!retorno_exportacion.Exito)
                    return retorno_exportacion;

                Configuracion.RutaFicheroJSON = retorno_exportacion.TextoDesarrollador;
            }
            else
            {
                return new RetornoBase(false, "No se ha podido generar el código de la clase.", "La ruta del fichero JSON no está definida.");
            }

            if (!File.Exists(Configuracion.RutaFicheroJSON))
                return new RetornoBase(false, "No se ha podido generar el código de la clase.", "El fichero JSON no existe en la ruta: " + Configuracion.RutaFicheroJSON);

            return GenerarCodigo(param_clase);
        }

        public async Task<RetornoBase> GenerarCodigoIA(Clase param_clase)
        {
            IProveedorIA proveedorIA = FactoriaProveedorIA.CrearProveedorIA(ProveedorIA.OpenAI);
            RetornoBase retorno_sin_ia = GenerarCodigo(param_clase);
            if (!retorno_sin_ia.Exito)
                return retorno_sin_ia;

            RetornoBase retorno_con_ia = await proveedorIA.GenerarCodigo(Configuracion.Proyecto, param_clase, retorno_sin_ia.TextoDesarrollador);
            if (!retorno_con_ia.Exito)
                return retorno_con_ia;

            RetornoBase retorno_completo = GenerarCodigoCompleto(retorno_sin_ia.TextoDesarrollador,
                retorno_con_ia.TextoDesarrollador);

            return retorno_completo;
        }

        private RetornoBase GenerarCodigoCompleto(string param_codigo_sin_ia, string param_codigo_con_ia)
        {
            if (string.IsNullOrEmpty(param_codigo_sin_ia))
                return new RetornoBase(false, "No se ha podido generar el código completo.", "El código base está vacío.");

            if (string.IsNullOrWhiteSpace(param_codigo_con_ia))
                return new RetornoBase(false, "No se ha podido generar el código completo.", "La respuesta de la IA está vacía.");

            try
            {
                using (JsonDocument json_document = JsonDocument.Parse(param_codigo_con_ia))
                {
                    JsonElement elemento_raiz = json_document.RootElement;

                    if (!elemento_raiz.TryGetProperty("codigo", out JsonElement json_codigo) || json_codigo.ValueKind != JsonValueKind.Array)
                    {
                        return new RetornoBase(false,
                            "El JSON generado por la IA no tiene el formato esperado.",
                            "No se ha encontrado la propiedad 'codigo' o no contiene un array.");
                    }

                    string codigo_completo = param_codigo_sin_ia;
                    foreach (JsonElement elemento_json in json_codigo.EnumerateArray())
                    {
                        if (!elemento_json.TryGetProperty("id", out JsonElement json_id) ||
                            !elemento_json.TryGetProperty("codigo", out JsonElement json_contenido))
                        {
                            return new RetornoBase(false,
                                "El JSON generado por la IA no tiene el formato esperado.",
                                "Uno de los elementos no contiene las propiedades 'id' y 'codigo'.");
                        }

                        string id = json_id.GetString();
                        string codigo = json_contenido.GetString();
                        if (string.IsNullOrWhiteSpace(id))
                        {
                            return new RetornoBase(false,
                                "El JSON generado por la IA contiene un identificador vacío.",
                                "No se puede sustituir un método sin identificador.");
                        }

                        string indicador = "//{{" + id + "}}";
                        if (!codigo_completo.Contains(indicador))
                        {
                            return new RetornoBase(false,
                                "No se ha podido insertar todo el código generado por la IA.",
                                "No se ha encontrado el indicador " + indicador + "en el código base.");
                        }

                        codigo_completo = codigo_completo.Replace(indicador, codigo ?? string.Empty);
                    }

                    return new RetornoBase(true, "Generación correcta.", codigo_completo);
                }
            }
            catch (JsonException ex)
            {
                return new RetornoBase(false, "Error al interpretar el JSON generado por la IA.", ex.Message);
            }
        }
    }
}
