using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using transformador_mda.src.Dominio;
using transformador_mda.src.Features.DAO;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Features.Utilidades;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Features.GenerarSalida;
using transformador_mda.src.Features.IntegracionIA;
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

        #region "Métodos de generación de código"

        /// <summary>
        /// A partir de una instancia del objeto Clase genera el esqueleto de una clase.
        /// </summary>
        /// <param name="param_clase">Información de un objeto de tipo clase.</param>
        /// <param name="param_barra_progreso">Barra de progreso de la vista para que el usuario vea que el proceso no está bloqueado.</param>
        /// <returns>Objeto con información del proceso, si todo sale bien el esqueleto de la clase, si hay error información sobre el error.</returns>
        public RetornoBase GenerarEsqueletoClase(Clase param_clase, ToolStripProgressBar param_barra_progreso)
        {
            GenerarSalidaBase generador = FactoriaSalida.CrearGenerarSalida(Configuracion);
            string texto = generador.GenerarTexto(param_clase, param_barra_progreso).TextoDesarrollador;

            return new RetornoBase(true, "Generación correcta", texto);
        }

        /// <summary>
        /// A partir de una instancia del objeto clase genera el código completo de una clase. 
        /// </summary>
        /// <param name="param_clase">Información de un objeto de tipo clase cuyo código se va a generar.</param>
        /// <param name="param_generar_json">True para generar el fichero JSON del proyecto, false para no generarlo.</param>
        /// <param name="param_barra_progreso">Barra de progreso de la vista para que el usuario vea que el proceso no está bloqueado.</param>
        /// <param name="param_forzar">True para forzar la generación del código si no están todos los comentarios, false para obligar a que estén los comentarios</param>
        /// <returns>Objeto con información del proceso, si todo sale bien el esqueleto de la clase, si hay error información sobre el error.</returns>
        public async Task<RetornoMultiple> GenerarCodigoIAClase(Clase param_clase, bool param_generar_json, ToolStripProgressBar param_barra_progreso, bool param_forzar = false)
        {
            // Barra de carga
            param_barra_progreso.Maximum = 11;
            param_barra_progreso.Value = 0;

            // Comprobación de comentarios
            if (!param_forzar)
            {
                RetornoMultiple retorno_comprobacion = ComprobarComentarios();
                if (!retorno_comprobacion.Exito)
                    return retorno_comprobacion;
            }
            param_barra_progreso.Value++;

            // Generación de JSON del proyecto
            if (param_generar_json)
            {
                RetornoMultiple retorno_generacion = GenerarFicheroProyecto();
                if (!retorno_generacion.Exito)
                    return retorno_generacion;
            }
            if (!File.Exists(Configuracion.RutaFicheroJSON))
                return new RetornoMultiple(false,
                    RetornoMultiple.EnumEstado.ERROR,
                    "No se ha podido generar el código de la clase.",
                    "El fichero JSON no existe en la ruta: " + Configuracion.RutaFicheroJSON);
            param_barra_progreso.Value++;

            // Generación de esqueleto
            RetornoBase retorno_sin_ia = GenerarEsqueletoClase(param_clase, null);
            if (!retorno_sin_ia.Exito)
                return new RetornoMultiple(retorno_sin_ia.Exito,
                    RetornoMultiple.EnumEstado.ERROR,
                    retorno_sin_ia.TextoUsuario,
                    retorno_sin_ia.TextoDesarrollador);
            param_barra_progreso.Value++;

            // Generación de código IA
            IProveedorIA proveedorIA = FactoriaProveedorIA.CrearProveedorIA(ProveedorIA.OpenAI);
            RetornoBase retorno_con_ia = await proveedorIA.GenerarCodigo(Configuracion.Proyecto, param_clase, retorno_sin_ia.TextoDesarrollador, param_barra_progreso);
            if (!retorno_con_ia.Exito)
                return new RetornoMultiple(retorno_con_ia.Exito,
                    RetornoMultiple.EnumEstado.ERROR,
                    retorno_con_ia.TextoUsuario,
                    retorno_con_ia.TextoDesarrollador);

            // Combinación de código
            return GenerarCodigoCompleto(retorno_sin_ia.TextoDesarrollador,
                retorno_con_ia.TextoDesarrollador);
        }

        /// <summary>
        /// Método para generar el fichero JSON del proyecto.
        /// </summary>
        /// <returns>Retorno con información del proceso.</returns>
        private RetornoMultiple GenerarFicheroProyecto()
        {
            if (File.Exists(Configuracion.RutaFicheroJSON))
            {
                File.Delete(Configuracion.RutaFicheroJSON);
            }

            if (Configuracion.Proyecto == null)
                return new RetornoMultiple(false,
                    RetornoMultiple.EnumEstado.ERROR,
                    "No se ha podido generar el código de la clase.",
                    "El proyecto está vacío.");

            RetornoBase retorno_exportacion = ProyectoDAO.Exportar(Configuracion.Proyecto, Application.StartupPath);
            if (!retorno_exportacion.Exito)
            {
                return new RetornoMultiple(retorno_exportacion.Exito,
                    RetornoMultiple.EnumEstado.ERROR,
                    retorno_exportacion.TextoUsuario,
                    retorno_exportacion.TextoDesarrollador);
            }

            Configuracion.RutaFicheroJSON = retorno_exportacion.TextoDesarrollador;
            return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CORRECTO, string.Empty);
        }

        /// <summary>
        /// Método para combinar el esqueleto y el código generado por la IA. 
        /// </summary>
        /// <param name="param_codigo_sin_ia">Código esqueleto de una clase.</param>
        /// <param name="param_codigo_con_ia">Código generado por la IA de una clase.</param>
        /// <returns>Objeto con información del proceso, si todo sale bien el esqueleto de la clase, si hay error información sobre el error.</returns>
        private RetornoMultiple GenerarCodigoCompleto(string param_codigo_sin_ia, string param_codigo_con_ia)
        {
            if (string.IsNullOrEmpty(param_codigo_sin_ia))
                return new RetornoMultiple(false,
                    RetornoMultiple.EnumEstado.ERROR,
                    "No se ha podido generar el código completo.",
                    "El código base está vacío.");

            if (string.IsNullOrWhiteSpace(param_codigo_con_ia))
                return new RetornoMultiple(false,
                    RetornoMultiple.EnumEstado.ERROR,
                    "No se ha podido generar el código completo.",
                    "La respuesta de la IA está vacía.");

            try
            {
                using (JsonDocument json_document = JsonDocument.Parse(param_codigo_con_ia))
                {
                    JsonElement elemento_raiz = json_document.RootElement;

                    if (!elemento_raiz.TryGetProperty("codigo", out JsonElement json_codigo) || json_codigo.ValueKind != JsonValueKind.Array)
                    {
                        return new RetornoMultiple(false,
                            RetornoMultiple.EnumEstado.ERROR,
                            "El JSON generado por la IA no tiene el formato esperado.",
                            "No se ha encontrado la propiedad 'codigo' o no contiene un array.");
                    }

                    string codigo_completo = param_codigo_sin_ia;
                    foreach (JsonElement elemento_json in json_codigo.EnumerateArray())
                    {
                        if (!elemento_json.TryGetProperty("id", out JsonElement json_id) ||
                            !elemento_json.TryGetProperty("codigo", out JsonElement json_contenido))
                        {
                            return new RetornoMultiple(false,
                                RetornoMultiple.EnumEstado.ERROR,
                                "El JSON generado por la IA no tiene el formato esperado.",
                                "Uno de los elementos no contiene las propiedades 'id' y 'codigo'.");
                        }

                        string id = json_id.GetString();
                        string codigo = json_contenido.GetString();
                        if (string.IsNullOrWhiteSpace(id))
                        {
                            return new RetornoMultiple(false,
                                RetornoMultiple.EnumEstado.ERROR,
                                "El JSON generado por la IA contiene un identificador vacío.",
                                "No se puede sustituir un método sin identificador.");
                        }

                        string indicador = "//{{" + id + "}}";
                        if (!codigo_completo.Contains(indicador))
                        {
                            return new RetornoMultiple(false,
                                RetornoMultiple.EnumEstado.ERROR,
                                "No se ha podido insertar todo el código generado por la IA.",
                                "No se ha encontrado el indicador " + indicador + "en el código base.");
                        }

                        codigo_completo = codigo_completo.Replace(indicador, codigo ?? string.Empty);
                    }

                    return new RetornoMultiple(true,
                        RetornoMultiple.EnumEstado.CORRECTO,
                        "Generación correcta.",
                        codigo_completo);
                }
            }
            catch (JsonException ex)
            {
                return new RetornoMultiple(false,
                    RetornoMultiple.EnumEstado.ERROR,
                    "Error al interpretar el JSON generado por la IA.",
                    ex.Message);
            }
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

        #region "Comprobar comentarios"

        /// <summary>
        /// Método principal para comprobar los comentarios de todo el objeto Proyecto.
        /// </summary>
        /// <returns>Objeto con información del proceso y el resultado de la comprobación.</returns>
        private RetornoMultiple ComprobarComentarios()
        {
            if (Configuracion.Proyecto == null)
                return new RetornoMultiple(false, 
                    RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO, 
                    "No se ha podido generar el código de la clase.", 
                    "El proyecto está vacío.");

            string mensaje_usuario = "La generación de código con IA funciona mejor si todos los elementos están comentados.\n" +
                        "¿Quiere continuar el proceso sin comentar todos los elementos UML?";

            /* No se permite al usuario rellenar este parámetro aún
            if (string.IsNullOrEmpty(Configuracion.Proyecto.Comentario))
                return new RetornoMultiple(false,
                    RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO, 
                    mensaje_usuario, 
                    "El proyecto no tiene descripción.");*/

            foreach (Directorio directorio in Configuracion.Proyecto.Directorios)
            {
                RetornoMultiple retorno_recursivo = ComprobarComentarios(directorio, mensaje_usuario);
                if (!retorno_recursivo.Exito)
                    return retorno_recursivo;
            }

            return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CORRECTO, string.Empty);
        }

        /// <summary>
        /// Método recursivo, comprueba el comentario de un directorio y todos sus elementos UML y hace un bucle revursivo por sus subdirectorios.
        /// </summary>
        /// <param name="param_directorio">Elemento UML a comprobar.</param>
        /// <param name="param_mensaje_usuario">Mensaje de error a devolver si hay algún elemento sin comentario.</param>
        /// <returns>Objeto con información del proceso y el resultado de la comprobación.</returns>
        private RetornoMultiple ComprobarComentarios(Directorio param_directorio, string param_mensaje_usuario)
        {
            /* No se permite al usuario modificar este parámetro aún
            if (string.IsNullOrEmpty(param_directorio.Comentario))
                return new RetornoMultiple(false,
                    RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO,
                    param_mensaje_usuario,
                    "El directorio " + param_directorio.Nombre + " no tiene descripción");*/

            foreach (Clase clase in param_directorio.Clases)
            {
                RetornoMultiple retorno_temporal = ComprobarComentarios(clase, param_mensaje_usuario);
                if (!retorno_temporal.Exito)
                    return retorno_temporal;

                retorno_temporal = ComprobarComentarios(clase.Atributos, param_mensaje_usuario);
                if (!retorno_temporal.Exito)
                    return retorno_temporal;

                retorno_temporal = ComprobarComentarios(clase.Literales, param_mensaje_usuario);
                if (!retorno_temporal.Exito)
                    return retorno_temporal;

                retorno_temporal = ComprobarComentarios(clase.Asociaciones, param_mensaje_usuario);
                if (!retorno_temporal.Exito)
                    return retorno_temporal;

                retorno_temporal = ComprobarComentarios(clase.Metodos, param_mensaje_usuario);
                if (!retorno_temporal.Exito)
                    return retorno_temporal;
            }

            foreach (Directorio subdirectorio in param_directorio.Subdirectorios)
            {
                RetornoMultiple retorno_temporal = ComprobarComentarios(subdirectorio, param_mensaje_usuario);
                if (!retorno_temporal.Exito)
                    return retorno_temporal;
            }

            return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CORRECTO, string.Empty);
        }

        /// <summary>
        /// Método que comprueba el comentario de una clase.
        /// </summary>
        /// <param name="param_clase">Elemento UML a comprobar.</param>
        /// <param name="param_mensaje_usuario">Mensaje de error a devolver si hay algún elemento sin comentario.</param>
        /// <returns>Objeto con información del proceso y el resultado de la comprobación.</returns>
        private RetornoMultiple ComprobarComentarios(Clase param_clase, string param_mensaje_usuario)
        {
            if (string.IsNullOrEmpty(param_clase.Comentario))
                return new RetornoMultiple(false,
                    RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO,
                    param_mensaje_usuario,
                    "La clase " + param_clase.Nombre + " no tiene descripción.");

            return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CORRECTO, string.Empty);
        }

        /// <summary>
        /// Método que comprueba el comentario de un listado de atributos
        /// </summary>
        /// <param name="param_atributos">Elemento UML a comprobar.</param>
        /// <param name="param_mensaje_usuario">Mensaje de error a devolver si hay algún elemento sin comentario.</param>
        /// <returns>Objeto con información del proceso y el resultado de la comprobación.</returns>
        private RetornoMultiple ComprobarComentarios(List<Atributo> param_atributos, string param_mensaje_usuario)
        {
            foreach (Atributo atributo in param_atributos)
            {
                if (string.IsNullOrEmpty(atributo.Comentario))
                    return new RetornoMultiple(false,
                        RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO,
                        param_mensaje_usuario,
                        "El atributo " + atributo.Nombre + " no tiene descripción.");
            }

            return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CORRECTO, string.Empty);
        }

        /// <summary>
        /// Método que comprueba el comentario de un listado de literales
        /// </summary>
        /// <param name="param_literales">Elemento UML a comprobar.</param>
        /// <param name="param_mensaje_usuario">Mensaje de error a devolver si hay algún elemento sin comentario.</param>
        /// <returns>Objeto con información del proceso y el resultado de la comprobación.</returns>
        private RetornoMultiple ComprobarComentarios(List<ElementoBase> param_literales, string param_mensaje_usuario)
        {
            foreach (ElementoBase literal in param_literales)
            {
                if (string.IsNullOrEmpty(literal.Comentario))
                    return new RetornoMultiple(false,
                        RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO,
                        param_mensaje_usuario,
                        "El literal " + literal.Nombre + " no tiene descripción.");
            }

            return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CORRECTO, string.Empty);
        }

        /// <summary>
        /// Método que comprueba el comentario de un listado de asociaciones
        /// </summary>
        /// <param name="param_asociaciones">Elemento UML a comprobar.</param>
        /// <param name="param_mensaje_usuario">Mensaje de error a devolver si hay algún elemento sin comentario.</param>
        /// <returns>Objeto con información del proceso y el resultado de la comprobación.</returns>
        private RetornoMultiple ComprobarComentarios(List<Asociacion> param_asociaciones, string param_mensaje_usuario)
        {
            foreach (Asociacion asociacion in param_asociaciones)
            {
                if (string.IsNullOrEmpty(asociacion.Comentario))
                    return new RetornoMultiple(false,
                        RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO,
                        param_mensaje_usuario,
                        "La asociación " + asociacion.Nombre + " no tiene descripción.");
            }

            return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CORRECTO, string.Empty);
        }

        /// <summary>
        /// Método que comprueba el comentario de un listado de métodos y de sus elementos UML.
        /// </summary>
        /// <param name="param_metodos">Elemento UML a comprobar.</param>
        /// <param name="param_mensaje_usuario">Mensaje de error a devolver si hay algún elemento sin comentario.</param>
        /// <returns>Objeto con información del proceso y el resultado de la comprobación.</returns>
        private RetornoMultiple ComprobarComentarios(List<Metodo> param_metodos, string param_mensaje_usuario)
        {
            foreach (Metodo metodo in param_metodos)
            {
                if (string.IsNullOrEmpty(metodo.Comentario))
                    return new RetornoMultiple(false,
                        RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO,
                        param_mensaje_usuario,
                        "El método " + metodo.Nombre + " no tiene descripción.");

                if (string.IsNullOrEmpty(metodo.Retorno.Comentario) && metodo.Retorno.TipoDato != EnumTipoDato.NO_DEFINIDO)
                    return new RetornoMultiple(false,
                        RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO,
                        param_mensaje_usuario,
                        "El retorno del método " + metodo.Nombre + " no tiene descripción.");

                RetornoMultiple retorno_temporal = ComprobarComentarios(metodo.Parametros, param_mensaje_usuario);
                if (!retorno_temporal.Exito)
                    return retorno_temporal;
            }

            return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CORRECTO, string.Empty);
        }

        /// <summary>
        /// Método que comprueba el comentario de un listado de parámetros
        /// </summary>
        /// <param name="param_parametros">Elemento UML a comprobar.</param>
        /// <param name="param_mensaje_usuario">Mensaje de error a devolver si hay algún elemento sin comentario.</param>
        /// <returns>Objeto con información del proceso y el resultado de la comprobación.</returns>
        private RetornoMultiple ComprobarComentarios(List<Parametro> param_parametros, string param_mensaje_usuario)
        {
            foreach (Parametro parametro in param_parametros)
            {
                if (string.IsNullOrEmpty(parametro.Comentario))
                    return new RetornoMultiple(false,
                        RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO,
                        param_mensaje_usuario,
                        "El parámentro " + parametro.Nombre + " no tiene descripción.");
            }

            return new RetornoMultiple(true, RetornoMultiple.EnumEstado.CORRECTO, string.Empty);
        }

        #endregion
    }
}
