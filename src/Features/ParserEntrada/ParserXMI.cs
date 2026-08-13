using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Features.Utilidades;
using transformador_mda.src.Dominio.ElementosUML;

namespace transformador_mda.src.Features.ParserEntrada
{
    /// <summary>
    /// Clase encargada de parsear un fichero XMI y construir un modelo de proyecto a partir de su contenido.
    /// </summary>
    public class ParserXMI : ParserUMLBase
    {
        #region "Atributos"

        /// <summary>
        /// Clave del XMI que se está procesando
        /// </summary>
        private ClavesSoportadasXMI.Enumeracion _clave_actual;

        /// <summary>
        /// Datos leídos del XMI que se está leyendo.
        /// </summary>
        private ProyectoBase _proyecto;

        /// <summary>
        /// Datos del documento XMI en formato XML
        /// </summary>
        private XDocument _documento;

        /// <summary>
        /// Datos del elemento que se está procesando en la lectura del documento.
        /// </summary>
        private XElement _elemento_actual;

        /// <summary>
        /// Asociaciones declaradas a nivel de paquete. Se enlazan al final, cuando ya existen todas las clases.
        /// </summary>
        private List<Asociacion> _asociaciones_pendientes;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Constructor de la clase, inicializa atributos.
        /// </summary>
        /// <param name="param_nombre">Nombre del proyecto.</param>
        /// <param name="param_ruta_fichero">Ruta del fichero XMI.</param>
        public ParserXMI(string param_nombre, string param_ruta_fichero) : base(param_ruta_fichero)
        {
            _proyecto = new ProyectoBase();
            _proyecto.Nombre = param_nombre;
            _asociaciones_pendientes = new List<Asociacion>();
        }

        #endregion

        #region "Métodos públicos"

        /// <summary>
        /// Lee el fichero XMI y construye un modelo de proyecto a partir de su contenido.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso de lectura y errores, si los hubiera.</returns>
        public override RetornoElementoUML Parsear()
        {
            if (string.IsNullOrEmpty(_ruta_fichero))
                return new RetornoElementoUML(false, "Ruta del fichero XMI no especificada.", null);

            _documento = XDocument.Load(_ruta_fichero);
            if (_documento == null)
                return new RetornoElementoUML(false, "No se pudo cargar el documento XMI.", null);

            if (!EsFormatoValido())
                return new RetornoElementoUML(false, "El formato del documento XMI no es válido.", null);

            return LeerFichero();
        }

        #endregion

        #region "Métodos protegidos"

        /// <summary>
        /// Comprueba si el formato del fichero abierto corresponde con el formato XMI que el generador lee.
        /// </summary>
        /// <returns>True si el formato es válido, false si no.</returns>
        protected bool EsFormatoValido()
        {
            if (_documento == null || _documento.Root == null)
                return false;

            bool tieneRaizValida =
                _documento.Root.Name.LocalName == "Model" ||
                _documento.Root.Name.LocalName == "XMI";

            if (!tieneRaizValida)
                return false;

            bool contieneModelo =
                _documento.Root.Name.LocalName == "Model" ||
                _documento.Descendants().Any(e => e.Name.LocalName == "Model");

            if (!contieneModelo)
                return false;

            bool contieneClavesSoportadas = _documento.Descendants().Any(e =>
                e.Name.LocalName == "packagedElement" ||
                e.Name.LocalName == "ownedAttribute" ||
                e.Name.LocalName == "ownedOperation" ||
                e.Name.LocalName == "ownedParameter" ||
                e.Name.LocalName == "generalization" ||
                e.Name.LocalName == "interfaceRealization");

            return contieneClavesSoportadas;
        }

        /// <summary>
        /// Método principal de lectura del fichero. Procesa el subconjunto de claves legibles y los lee de manera recursiva.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto ProyectoBase con la información del XMI.</returns>
        protected RetornoElementoUML LeerFichero()
        {
            XElement modelo = _documento.Descendants().FirstOrDefault(e => e.Name.LocalName == "Model");
            if (modelo == null)
                return new RetornoElementoUML(false,
                    "No se ha podido procesar el documento correctamente.",
                    null,
                    "No se encontró el elemento 'Model' en el documento XMI.");

            foreach (XElement hijo in ObtenerHijosProcesables(modelo))
            {
                _elemento_actual = hijo;
                _clave_actual = ClavesSoportadasXMI.ObtenerEnumClave(ObtenerClaveXMI(hijo));

                RetornoElementoUML resultado = ProcesarElemento();
                if (!resultado.Exito)
                    return resultado;

                if (resultado.Elemento is Directorio directorio)
                    _proyecto.Directorios.Add(directorio);
                else if (resultado.Elemento is Clase clase)
                    _proyecto.Directorios.FirstOrDefault()?.Clases.Add(clase);
                else if (resultado.Elemento is Asociacion asociacion)
                    _asociaciones_pendientes.Add(asociacion);
            }

            EnlazarAsociacionesPendientes();

            RetornoBase retornoEstereotipos = ProcesarConstructoresYDestructores();
            if (!retornoEstereotipos.Exito)
            {
                return new RetornoElementoUML(
                    false,
                    retornoEstereotipos.TextoUsuario,
                    null,
                    retornoEstereotipos.TextoDesarrollador);
            }

            return new RetornoElementoUML(true, "Fichero XMI leído correctamente.", _proyecto);
        }

        /// <summary>
        /// Procesa el tipo de clave que se ha leido y redirige al método que lo procesa.
        /// </summary>
        /// <param name="param_es_subdirectorio">Indica si el elemento que se está procesando es un subdirectorio.</param>
        /// <exception cref="Exception">No se reconoce el elemento de la enumeración..</exception>
        /// <returns>True si el elemento se procesa correctamente, false en caso contrario.</returns>
        protected RetornoElementoUML ProcesarElemento()
        {
            RetornoElementoUML retorno = new RetornoElementoUML();
            retorno.Exito = true;
            switch (_clave_actual)
            {
                case ClavesSoportadasXMI.Enumeracion.PAQUETE:
                    retorno = ProcesarDirectorio();
                    break;

                case ClavesSoportadasXMI.Enumeracion.CLASE:
                    retorno = ProcesarClase();
                    break;

                case ClavesSoportadasXMI.Enumeracion.ATRIBUTO:
                    retorno = ProcesarAtributo();
                    break;

                case ClavesSoportadasXMI.Enumeracion.PARAMETRO:
                    retorno = ProcesarParametro();
                    break;

                case ClavesSoportadasXMI.Enumeracion.METODO:
                    retorno = ProcesarMetodo();
                    break;

                case ClavesSoportadasXMI.Enumeracion.ASOCIACION:
                    retorno = ProcesarAsociacion();
                    break;

                case ClavesSoportadasXMI.Enumeracion.HERENCIA:
                    retorno = ProcesarHerencia();
                    break;

                case ClavesSoportadasXMI.Enumeracion.LITERAL:
                    retorno = ProcesarLiteral();
                    break;

                default:
                    throw new Exception("Clave no soportada: " + _clave_actual);
            }

            return retorno;
        }

        /// <summary>
        /// Método encargado de leer y extraer la información de los directorios, carga el objeto Directorio con todos sus descendientes y delega la carga de su propia instancia al objeto superior (el proyecto)
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto Directorio con información del XMI.</returns>
        protected RetornoElementoUML ProcesarDirectorio()
        {
            Directorio nuevo_nodo = new Directorio()
            {
                Id = ObtenerID(_elemento_actual),
                Nombre = ObtenerNombre(_elemento_actual),
                Comentario = ObtenerComentario(_elemento_actual),
                Tipo = EnumTipoElemento.DIRECTORIO,

                Namespace = _elemento_actual.Attribute("name")?.Value ?? "DirectorioSinNombre",
                Subdirectorios = new List<Directorio>(),
                Clases = new List<Clase>()
            };

            foreach (XElement hijo in ObtenerHijosProcesables(_elemento_actual))
            {
                _elemento_actual = hijo;
                _clave_actual = ClavesSoportadasXMI.ObtenerEnumClave(ObtenerClaveXMI(hijo));

                RetornoElementoUML resultado = ProcesarElemento();
                if (!resultado.Exito)
                    return resultado;

                if (resultado.Elemento is Directorio subdirectorio)
                    nuevo_nodo.Subdirectorios.Add(subdirectorio);
                else if (resultado.Elemento is Clase clase)
                    nuevo_nodo.Clases.Add(clase);
                else if (resultado.Elemento is Asociacion asociacion)
                    _asociaciones_pendientes.Add(asociacion);
            }

            return new RetornoElementoUML() { Exito = true, Elemento = nuevo_nodo };
        }

        /// <summary>
        /// Método encargado de leer y extraer la información de las clases, carga el objeto Clase con todos sus descendientes y delega la carga de su propia instancia al objeto superior (el directorio o el proyecto)
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto Clase con información del XMI.</returns>
        protected RetornoElementoUML ProcesarClase()
        {
            EnumTiposClases tipo_clase = EnumTiposClases.NO_DEFINIDA;
            string clave_xmi = ObtenerClaveXMI(_elemento_actual);
            if (clave_xmi == "packagedElement xmi:type=\"uml:Class\"")
                tipo_clase = ObtenerTipoClase("uml:Class", ObtenerBooleano(_elemento_actual, "isAbstract"));
            else if (clave_xmi == "packagedElement xmi:type=\"uml:Enumeration\"")
                tipo_clase = EnumTiposClases.ENUMERACION;
            else if (clave_xmi == "packagedElement xmi:type=\"uml:Interface\"")
                tipo_clase = EnumTiposClases.INTERFAZ;
            else
                return new RetornoElementoUML(false,
                    "No se ha podido procesar el documento XMI", null,
                    "Tipo de clase no soportado: " + clave_xmi);

            Clase nuevo_nodo = new Clase()
            {
                Id = ObtenerID(_elemento_actual),
                Nombre = ObtenerNombre(_elemento_actual),
                Comentario = ObtenerComentario(_elemento_actual),
                Tipo = EnumTipoElemento.CLASE,
                Visibilidad = ObtenerVisibilidad(_elemento_actual),

                TipoClase = tipo_clase,
                Atributos = new List<Atributo>(),
                Metodos = new List<Metodo>(),
                Asociaciones = new List<Asociacion>(),
                Literales = new List<ElementoBase>()
            };

            foreach (XElement hijo in ObtenerHijosProcesables(_elemento_actual))
            {
                _elemento_actual = hijo;
                _clave_actual = ClavesSoportadasXMI.ObtenerEnumClave(ObtenerClaveXMI(hijo));

                RetornoElementoUML resultado = ProcesarElemento();
                if (!resultado.Exito)
                    return resultado;
                if (resultado.Elemento is Atributo atributo)
                    nuevo_nodo.Atributos.Add(atributo);
                else if (resultado.Elemento is Metodo metodo)
                    nuevo_nodo.Metodos.Add(metodo);
                else if (resultado.Elemento is Asociacion asociacion)
                    nuevo_nodo.Asociaciones.Add(asociacion);
                else if (resultado.Elemento is ElementoBase literal)
                    nuevo_nodo.Literales.Add(literal);
            }

            return new RetornoElementoUML() { Exito = true, Elemento = nuevo_nodo };
        }

        /// <summary>
        /// Método encargado de leer y extraer la información de los atributos, carga el objeto Atributo y delega la carga de su propia instancia al objeto superior (el objeto Clase)
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto Atributo con información del XMI.</returns>
        protected RetornoElementoUML ProcesarAtributo()
        {
            Atributo nuevo_nodo = new Atributo()
            {
                Id = ObtenerID(_elemento_actual),
                Nombre = ObtenerNombre(_elemento_actual),
                Comentario = ObtenerComentario(_elemento_actual),
                Tipo = EnumTipoElemento.ATRIBUTO,
                TipoDato = ObtenerTipoDeDato(_elemento_actual),
                Visibilidad = ObtenerVisibilidad(_elemento_actual)
            };

            return new RetornoElementoUML() { Exito = true, Elemento = nuevo_nodo };
        }

        /// <summary>
        /// Método encargado de leer y extraer la información de los métodos, carga el objeto Metodo con todos sus descendientes y delega la carga de su propia instancia al objeto superior (el objeto Clase)
        /// </summary>
        /// <remarks>
        /// Comprueba si hay algún parametro de tipo return para no tratarlo como parámetro sino como tipo de dato de retorno.
        /// </remarks>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto Metodo con información del XMI.</returns>
        protected RetornoElementoUML ProcesarMetodo()
        {
            Metodo nuevo_nodo = new Metodo()
            {
                Id = ObtenerID(_elemento_actual),
                Nombre = ObtenerNombre(_elemento_actual),
                Comentario = ObtenerComentario(_elemento_actual),
                Tipo = EnumTipoElemento.METODO,
                Visibilidad = ObtenerVisibilidad(_elemento_actual),
                Parametros = new List<Parametro>()
            };

            foreach (XElement hijo in ObtenerHijosProcesables(_elemento_actual))
            {
                _elemento_actual = hijo;
                _clave_actual = ClavesSoportadasXMI.ObtenerEnumClave(ObtenerClaveXMI(hijo));

                RetornoElementoUML resultado = ProcesarElemento();
                if (!resultado.Exito)
                    return resultado;

                if (resultado.Elemento.Tipo == EnumTipoElemento.PARAMETRO)
                    nuevo_nodo.Parametros.Add((Parametro)resultado.Elemento);
                else if (resultado.Elemento.Tipo == EnumTipoElemento.RETORNO)
                    nuevo_nodo.Retorno = (Parametro)resultado.Elemento;
            }

            return new RetornoElementoUML() { Exito = true, Elemento = nuevo_nodo };
        }

        /// <summary>
        /// Método encargado de leer y extraer la información de los parámetros, carga el objeto Parametro y delega la carga de su propia instncia al objeto superior (el objeto Método)
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto Parametro con información del XMI.</returns>
        protected RetornoElementoUML ProcesarParametro()
        {
            Parametro nuevo_nodo = new Parametro()
            {
                Id = ObtenerID(_elemento_actual),
                Nombre = ObtenerNombre(_elemento_actual),
                Comentario = ObtenerComentario(_elemento_actual),
                TipoDato = ObtenerTipoDeDato(_elemento_actual),
                Tipo = _elemento_actual.Attributes().FirstOrDefault(a => a.Name.LocalName == "direction")?.Value == "return" ?
                    EnumTipoElemento.RETORNO : EnumTipoElemento.PARAMETRO
            };

            return new RetornoElementoUML() { Exito = true, Elemento = nuevo_nodo };
        }

        /// <summary>
        /// Método encargado de leer y extraer la información de los literales, carga el objeto ElementoBase y delega la carga de su propia instancia al objeto superior (el objeto Clase)
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto ElementoBase con información del XMI.</returns>
        protected RetornoElementoUML ProcesarLiteral()
        {
            ElementoBase nuevo_literal = new ElementoBase()
            {
                Id = ObtenerID(_elemento_actual),
                Nombre = ObtenerNombre(_elemento_actual),
                Tipo = EnumTipoElemento.LITERAL,
                Comentario = ObtenerComentario(_elemento_actual)
            };

            return new RetornoElementoUML() { Exito = true, Elemento = nuevo_literal };
        }

        /// <summary>
        /// Método encargado de leer y extraer la información de las asociaciones, carga el objeto Asociacion y delega la carga de su propia instancia al objeto superior.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto Asociacion con información del XMI.</returns>
        protected RetornoElementoUML ProcesarAsociacion()
        {
            string tipo_xmi = ObtenerTipoXMI(_elemento_actual);

            switch (tipo_xmi)
            {
                case "uml:Association":
                case "uml:AssociationClass":
                    return ProcesarAsociacionBinaria();

                case "uml:Dependency":
                case "uml:Usage":
                case "uml:Abstraction":
                case "uml:Substitution":
                    return ProcesarDependencia();

                case "uml:Realization":
                case "uml:InterfaceRealization":
                    return ProcesarRealizacionInterfaz();

                default:
                    if (_elemento_actual.Name.LocalName == "interfaceRealization")
                        return ProcesarRealizacionInterfaz();

                    return new RetornoElementoUML(false,
                        "No se ha podido procesar el documento XMI", null,
                        "Tipo de asociación no soportado: " + tipo_xmi);
            }
        }

        /// <summary>
        /// Método encargado de leer y extraer la información de las asociaciones, carga el objeto Asociacion y delega la carga de su propia instancia al objeto superior.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto Asociacion con información del XMI.</returns>
        protected RetornoElementoUML ProcesarHerencia()
        {
            XElement clase_hija = ObtenerClasePropietaria(_elemento_actual);

            Asociacion nueva_asociacion = new Asociacion()
            {
                Tipo = EnumTipoElemento.ASOCIACION,
                Id = ObtenerID(_elemento_actual),
                Nombre = ObtenerNombre(_elemento_actual),
                Comentario = ObtenerComentario(_elemento_actual),
                TipoAsociacion = EnumTipoAsociacion.HERENCIA,
                IdClaseOrigen = ObtenerReferencia(_elemento_actual, "general"),
                IdClaseDestino = ObtenerID(clase_hija),
                EsNavegableOrigen = true,
                EsNavegableDestino = true
            };

            return new RetornoElementoUML() { Exito = true, Elemento = nueva_asociacion };
        }

        #endregion

        #region "Métodos privados para asociaciones"

        /// <summary>
        /// Lee asociaciones UML binarias de Modelio: asociación simple, dirigida, agregación y composición.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto Asociacion con información de una asociación binaria.</returns>
        private RetornoElementoUML ProcesarAsociacionBinaria()
        {
            List<ExtremoAsociacionXMI> extremos = ObtenerExtremosAsociacion(_elemento_actual);
            if (extremos.Count < 2)
                return new RetornoElementoUML(false,
                    "No se ha podido procesar el documento XMI", null,
                    "La asociación '" + ObtenerID(_elemento_actual) + "' no tiene suficientes extremos.");

            ExtremoAsociacionXMI extremo_propiedad = extremos
                .FirstOrDefault(e => !string.IsNullOrWhiteSpace(e.PropietarioClaseId));

            ExtremoAsociacionXMI extremo_origen;
            ExtremoAsociacionXMI extremo_destino;
            string id_clase_origen;
            string id_clase_destino;

            if (extremo_propiedad != null)
            {
                extremo_origen = extremos.FirstOrDefault(e => e != extremo_propiedad &&
                    e.ClaseId == extremo_propiedad.PropietarioClaseId) ?? extremo_propiedad;
                extremo_destino = extremo_propiedad;
                id_clase_origen = extremo_propiedad.PropietarioClaseId;
                id_clase_destino = extremo_propiedad.ClaseId;
            }
            else
            {
                extremo_origen = extremos[0];
                extremo_destino = extremos[1];
                id_clase_origen = extremo_origen.ClaseId;
                id_clase_destino = extremo_destino.ClaseId;
            }

            Asociacion nueva_asociacion = CrearAsociacionBase(EnumTipoAsociacion.ASOCIACION);
            nueva_asociacion.TipoAsociacion = ObtenerTipoAsociacionDesdeExtremos(extremos);
            nueva_asociacion.IdClaseOrigen = id_clase_origen ?? string.Empty;
            nueva_asociacion.IdClaseDestino = id_clase_destino ?? string.Empty;
            nueva_asociacion.MultiplicidadOrigen = ObtenerMultiplicidad(extremo_origen);
            nueva_asociacion.MultiplicidadDestino = ObtenerMultiplicidad(extremo_destino);
            nueva_asociacion.NombreRolOrigen = extremo_origen.NombreRol;
            nueva_asociacion.NombreRolDestino = extremo_destino.NombreRol;
            nueva_asociacion.EsNavegableOrigen = extremo_destino.EsNavegable;
            nueva_asociacion.EsNavegableDestino = extremo_origen.EsNavegable;

            return new RetornoElementoUML() { Exito = true, Elemento = nueva_asociacion };
        }

        /// <summary>
        /// Lee dependencias, usos y abstracciones. En UML el cliente depende del proveedor.
        /// </summary
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto Asociacion con información de una asociación de dependencia.</returns>
        private RetornoElementoUML ProcesarDependencia()
        {
            Asociacion nueva_asociacion = CrearAsociacionBase(EnumTipoAsociacion.DEPENDENCIA);
            nueva_asociacion.IdClaseOrigen = ObtenerPrimeraReferencia(_elemento_actual, "client");
            nueva_asociacion.IdClaseDestino = ObtenerPrimeraReferencia(_elemento_actual, "supplier");
            nueva_asociacion.EsNavegableOrigen = true;

            return new RetornoElementoUML() { Exito = true, Elemento = nueva_asociacion };
        }

        /// <summary>
        /// Lee realizaciones de interfaz. Para el generador se tratan como herencia hacia la interfaz.
        /// </summary>
        /// <returns>Objeto de tipo RetornoElementoUML con información del proceso y, si todo ha funcionado bien con el objeto Asociacion con información de una asociación de interfaz.</returns>
        private RetornoElementoUML ProcesarRealizacionInterfaz()
        {
            Asociacion nueva_asociacion = CrearAsociacionBase(EnumTipoAsociacion.HERENCIA);
            nueva_asociacion.IdClaseOrigen =
                ObtenerPrimeraReferencia(_elemento_actual, "contract") ??
                ObtenerPrimeraReferencia(_elemento_actual, "supplier");
            nueva_asociacion.IdClaseDestino =
                ObtenerPrimeraReferencia(_elemento_actual, "client") ??
                ObtenerID(ObtenerClasePropietaria(_elemento_actual));
            nueva_asociacion.EsNavegableOrigen = true;
            nueva_asociacion.EsNavegableDestino = true;

            return new RetornoElementoUML() { Exito = true, Elemento = nueva_asociacion };
        }

        /// <summary>
        /// Genera el tipo de dato Asociacion con la información de una asociación básica.
        /// </summary>
        /// <param name="param_tipo_asociacion">Tipo de asociación que se va a instanciar.</param>
        /// <returns>Objeto de tipo Asociaciion</returns>
        private Asociacion CrearAsociacionBase(EnumTipoAsociacion param_tipo_asociacion)
        {
            return new Asociacion()
            {
                Tipo = EnumTipoElemento.ASOCIACION,
                Id = ObtenerID(_elemento_actual),
                Nombre = ObtenerNombre(_elemento_actual),
                Comentario = ObtenerComentario(_elemento_actual),
                TipoAsociacion = param_tipo_asociacion
            };
        }

        /// <summary>
        /// Método encargado de obtener la clase extremo de la asociación que se está procesando.
        /// </summary>
        /// <param name="param_asociacion">Elemento de tipo asociación que se está procesando.</param>
        /// <returns>Listado con los extremos de una asociación.</returns>
        private List<ExtremoAsociacionXMI> ObtenerExtremosAsociacion(XElement param_asociacion)
        {
            List<ExtremoAsociacionXMI> extremos = new List<ExtremoAsociacionXMI>();

            foreach (string id_extremo in ObtenerReferencias(param_asociacion, "memberEnd"))
            {
                XElement elemento_extremo = BuscarElementoPorID(_documento, id_extremo);
                if (elemento_extremo != null)
                    extremos.Add(CrearExtremoAsociacion(elemento_extremo, param_asociacion));
            }

            foreach (XElement owned_end in param_asociacion.Elements().Where(e => e.Name.LocalName == "ownedEnd"))
            {
                string id_owned_end = ObtenerID(owned_end);
                if (!extremos.Any(e => e.Id == id_owned_end))
                    extremos.Add(CrearExtremoAsociacion(owned_end, param_asociacion));
            }

            return extremos
                .Where(e => !string.IsNullOrWhiteSpace(e.ClaseId) || !string.IsNullOrWhiteSpace(e.PropietarioClaseId))
                .ToList();
        }

        /// <summary>
        /// Genera un objeto de tipo extremo para una asociación.
        /// </summary>
        /// <param name="param_extremo">Elemento extremo de una asociación.</param>
        /// <param name="param_asociacion">Objeto con la información de la asociación.</param>
        /// <returns>Objeto con la información de un extremo de la asociación.</returns>
        private ExtremoAsociacionXMI CrearExtremoAsociacion(XElement param_extremo, XElement param_asociacion)
        {
            string id_extremo = ObtenerID(param_extremo);
            XElement clase_propietaria = ObtenerClasePropietaria(param_extremo);
            bool esta_en_navegables = ObtenerReferencias(param_asociacion, "navigableOwnedEnd").Contains(id_extremo);

            return new ExtremoAsociacionXMI()
            {
                Id = id_extremo,
                ClaseId = ObtenerReferencia(param_extremo, "type"),
                PropietarioClaseId = ObtenerID(clase_propietaria),
                NombreRol = ObtenerNombre(param_extremo),
                Agregacion = ObtenerAtributo(param_extremo, "aggregation"),
                LimiteInferior = ObtenerValorHijo(param_extremo, "lowerValue"),
                LimiteSuperior = ObtenerValorHijo(param_extremo, "upperValue"),
                EsNavegable = esta_en_navegables ||
                    ObtenerBooleano(param_extremo, "isNavigable") ||
                    (param_extremo.Name.LocalName == "ownedAttribute" && clase_propietaria != null)
            };
        }

        /// <summary>
        /// Según el tipo de asociación se devuelve un literal de una enumeración u otro.
        /// </summary>
        /// <param name="param_extremos">Listado de extremos de una asociación.</param>
        /// <returns>Literal de una enumeración que identifica el tipo de esta.</returns>
        private EnumTipoAsociacion ObtenerTipoAsociacionDesdeExtremos(List<ExtremoAsociacionXMI> param_extremos)
        {
            if (param_extremos.Any(e => e.Agregacion == "composite"))
                return EnumTipoAsociacion.COMPOSICION;

            if (param_extremos.Any(e => e.Agregacion == "shared"))
                return EnumTipoAsociacion.AGREGACION;

            return EnumTipoAsociacion.ASOCIACION;
        }

        /// <summary>
        /// Método encargado de buscar la multiplicidad de una asociación.
        /// </summary>
        /// <param name="param_extremo">Extremo de la asociación sobre el que hay que buscar la multiplicidad.</param>
        /// <returns>Multiplicidad leida.</returns>
        private string ObtenerMultiplicidad(ExtremoAsociacionXMI param_extremo)
        {
            if (param_extremo == null)
                return string.Empty;

            string inferior = param_extremo.LimiteInferior;
            string superior = param_extremo.LimiteSuperior;

            if (string.IsNullOrWhiteSpace(inferior) && string.IsNullOrWhiteSpace(superior))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(superior))
                return inferior;

            if (string.IsNullOrWhiteSpace(inferior) || inferior == superior)
                return superior;

            return inferior + ".." + superior;
        }

        /// <summary>
        /// Método encargado de enlazar las asociaciones con sus extremos.
        /// </summary>
        private void EnlazarAsociacionesPendientes()
        {
            foreach (Asociacion asociacion in _asociaciones_pendientes)
            {
                Clase clase_origen = ObtenerClasePorId(asociacion.IdClaseOrigen);
                Clase clase_destino = ObtenerClasePorId(asociacion.IdClaseDestino);

                AgregarAsociacionAClase(clase_origen, asociacion);
                AgregarAsociacionAClase(clase_destino, asociacion);
            }
        }

        /// <summary>
        /// Añade una asociación a una clase concreta.
        /// </summary>
        /// <param name="param_clase">Clase padre de la asociación.</param>
        /// <param name="param_asociacion">Asociación que unir a la clase.</param>
        private void AgregarAsociacionAClase(Clase param_clase, Asociacion param_asociacion)
        {
            if (param_clase == null || param_asociacion == null)
                return;

            if (param_clase.Asociaciones.Any(a => a.Id == param_asociacion.Id))
                return;

            param_clase.Asociaciones.Add(param_asociacion);
        }

        /// <summary>
        /// Método encargado de recorrer todos los directorios en busca de una clase según el id de la clase.
        /// </summary>
        /// <param name="param_id">Identificador de la clase a buscar.</param>
        /// <returns>Objeto de tipo clase con la información de la clase buscada</returns>
        private Clase ObtenerClasePorId(string param_id)
        {
            if (string.IsNullOrWhiteSpace(param_id))
                return null;

            foreach (Directorio directorio in _proyecto.Directorios)
            {
                Clase clase = ObtenerClasePorId(directorio, param_id);
                if (clase != null)
                    return clase;
            }

            return null;
        }

        /// <summary>
        /// Según un identificador dado busca la clase que tenga dicho identificador
        /// </summary>
        /// <param name="param_directorio">Directorio sobre el que hay que buscar la clase</param>
        /// <param name="param_id">Identificador de la clase a buscar</param>
        /// <returns>Clase con id pasado por parametro o null si no se encuentra.</returns>
        private Clase ObtenerClasePorId(Directorio param_directorio, string param_id)
        {
            if (param_directorio == null)
                return null;

            Clase clase = param_directorio.Clases.FirstOrDefault(c => c.Id == param_id);
            if (clase != null)
                return clase;

            foreach (Directorio subdirectorio in param_directorio.Subdirectorios)
            {
                clase = ObtenerClasePorId(subdirectorio, param_id);
                if (clase != null)
                    return clase;
            }

            return null;
        }

        /// <summary>
        /// Busca y obtiene la clase propietaria de una asociación.
        /// </summary>
        /// <param name="param_elemento">Elemento del XMI sobre el que hay que buscar quien es el padre de la asociación.</param>
        /// <returns>Elemento padre de la asociación que se está procesando.</returns>
        private XElement ObtenerClasePropietaria(XElement param_elemento)
        {
            if (param_elemento == null)
                return null;

            return param_elemento
                .Ancestors()
                .FirstOrDefault(e =>
                    EsTipoXMI(e, "uml:Class") ||
                    EsTipoXMI(e, "uml:Interface") ||
                    EsTipoXMI(e, "uml:Enumeration"));
        }

        /// <summary>
        /// DTO con la información de un extremo de una asociación.
        /// </summary>
        private class ExtremoAsociacionXMI
        {
            public string Id { get; set; }
            public string ClaseId { get; set; }
            public string PropietarioClaseId { get; set; }
            public string NombreRol { get; set; }
            public string Agregacion { get; set; }
            public string LimiteInferior { get; set; }
            public string LimiteSuperior { get; set; }
            public bool EsNavegable { get; set; }
        }

        #endregion

        #region "Métodos estáticos"

        /// <summary>
        /// Pasado un elemento XMI devuelve la clave que lo identifica, por ejemplo "packagedElement xmi:type=\"uml:Class\"".
        /// </summary>
        /// <param name="param_elemento">Elemento XMI del cual obtener la clave.</param>
        /// <returns>Clave del elemento.</returns>
        static public string ObtenerClaveXMI(XElement param_elemento)
        {
            string ret_clave = param_elemento.Name.LocalName;
            if (ret_clave != "packagedElement")
            {
                return ret_clave;
            }

            string tipoXmi = param_elemento.Attributes().FirstOrDefault(a => a.Name.LocalName == "type")?.Value;
            if (string.IsNullOrEmpty(tipoXmi))
            {
                return string.Empty;
            }

            ret_clave += $" xmi:type=\"{tipoXmi}\"";
            return ret_clave;
        }

        /// <summary>
        /// Obtiene el id de una línea XMI.
        /// </summary>
        /// <param name="param_elemento">Línea XMI a procesar para buscar el identificador.</param>
        /// <returns>Identificador si existe cadena vacía si no se encuentra.</returns>
        static public string ObtenerID(XElement param_elemento)
        {
            if (param_elemento == null)
                return string.Empty;

            return param_elemento
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == "id")
                ?.Value ?? string.Empty;
        }

        /// <summary>
        /// Obtiene el nombre de una línea XMI.
        /// </summary>
        /// <param name="param_elemento">Línea XMI a procesar para buscar el nombre.</param>
        /// <returns>Nombre si existe cadena vacía si no se encuentra.</returns>
        static public string ObtenerNombre(XElement param_elemento)
        {
            if (param_elemento == null)
                return string.Empty;

            return param_elemento
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == "name")
                ?.Value ?? string.Empty;
        }

        /// <summary>
        /// Obtiene el texto de un comentario asociado al elemento pasado por parámetro.
        /// </summary>
        /// <param name="param_elemento">Elemento cuyo comentario hay que extraer.</param>
        /// <returns>Texto del comentario asociado al elemento</returns>
        static public string ObtenerComentario(XElement param_elemento)
        {
            if (param_elemento == null)
                return string.Empty;

            XElement comentario = param_elemento.Elements().FirstOrDefault(e => e.Name.LocalName == "ownedComment");
            if (comentario == null)
                return string.Empty;

            XElement cuerpo = comentario.Elements().FirstOrDefault(c => c.Name.LocalName == "body");
            return cuerpo?.Value ?? string.Empty;
        }

        /// <summary>
        /// Busca un elemento en el documento XMI por su identificador.
        /// </summary>
        /// <param name="param_documento">Documento XMI donde buscar el elemento.</param>
        /// <param name="param_id">Identificador del elemento a buscar.</param>
        /// <returns>Elemento si se encuentra, null si no se encuentra.</returns>
        static public XElement BuscarElementoPorID(XDocument param_documento, string param_id)
        {
            if (param_documento == null || string.IsNullOrWhiteSpace(param_id))
                return null;

            return param_documento
                .Descendants()
                .FirstOrDefault(e => ObtenerID(e) == param_id);
        }

        /// <summary>
        /// Busca un elemento en el documento XMI por su clave.
        /// </summary>
        /// <param name="param_documento">Documento XMI donde buscar el elemento.</param>
        /// <param name="param_clave">Clave del elemento a buscar.</param>
        /// <returns>Elemento si se encuentra, null si no se encuentra.</returns>
        static public XElement BuscarElementoPorClave(XDocument param_documento, string param_clave)
        {
            if (param_documento == null || string.IsNullOrWhiteSpace(param_clave))
                return null;

            return param_documento
                .Descendants()
                .FirstOrDefault(e => e.Name.LocalName == param_clave);
        }

        /// <summary>
        /// Busca un elemento en el documento XMI por su nombre.
        /// </summary>
        /// <param name="param_documento">Documento XMI donde buscar el elemento.</param>
        /// <param name="param_nombre">Nombre del elemento a buscar.</param>
        /// <returns>Elemento si se encuentra, null si no se encuentra.</returns>
        static public XElement BuscarElementoPorNombre(XDocument param_documento, string param_nombre)
        {
            if (param_documento == null || string.IsNullOrWhiteSpace(param_nombre))
                return null;

            return param_documento
                .Descendants()
                .FirstOrDefault(e => e.Attribute("name")?.Value == param_nombre);
        }

        /// <summary>
        /// Devuelve un listado de los elementos procesables descendientes de un elemento pasado.
        /// </summary>
        /// <param name="param_elemento_padre">Elemento padre del que se extraerán los descendientes.</param>
        /// <returns>Vector de elementos.</returns>
        static public IEnumerable<XElement> ObtenerHijosProcesables(XElement param_elemento_padre)
        {
            if (param_elemento_padre == null)
                yield break;

            foreach (XElement hijo in param_elemento_padre.Elements())
            {
                string clave = ObtenerClaveXMI(hijo);
                if (ClavesSoportadasXMI.ObtenerEnumClave(clave) == ClavesSoportadasXMI.Enumeracion.NULO)
                    continue;

                yield return hijo;
            }
        }

        /// <summary>
        /// Obtiene el tipo de visibilidad asociado a un elemento pasado por parámetros.
        /// </summary>
        /// <param name="param_elemento">Elemento cuya visibilidad hay que extraer.</param>
        /// <returns>Enumeración con la visibilidad leída.</returns>
        static public EnumTipoVisibilidad ObtenerVisibilidad(XElement param_elemento)
        {
            string visibilidad = ObtenerAtributo(param_elemento, "visibility");
            if (string.IsNullOrEmpty(visibilidad))
                return EnumTipoVisibilidad.NO_DEFINIDA;

            return ObtenerTipoVisibilidad(visibilidad);
        }

        /// <summary>
        /// Obtiene el tipo de dato asociado al elemento pasado por parámetro.
        /// </summary>
        /// <param name="param_elemento">Elemento cuyo tipo de dato hay que extraer.</param>
        /// <returns>Enumeración con el tipo de dato.</returns>
        static public EnumTipoDato ObtenerTipoDeDato(XElement param_elemento)
        {
            XElement tipo = param_elemento.Elements().FirstOrDefault(e => e.Name.LocalName == "type");
            if (tipo == null)
                return EnumTipoDato.NO_DEFINIDO;

            string tipo_dato = ObtenerAtributo(tipo, "href");
            if (string.IsNullOrEmpty(tipo_dato))
                return EnumTipoDato.NO_DEFINIDO;

            return ObtenerTipoDato(tipo_dato);
        }

        /// <summary>
        /// Obtiene el valor de un atributo ignorando el namespace XML.
        /// </summary>
        static public string ObtenerAtributo(XElement param_elemento, string param_nombre_atributo)
        {
            if (param_elemento == null || string.IsNullOrWhiteSpace(param_nombre_atributo))
                return string.Empty;

            return param_elemento
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == param_nombre_atributo)
                ?.Value ?? string.Empty;
        }

        /// <summary>
        /// Obtiene el xmi:type de un elemento ignorando el namespace XML.
        /// </summary>
        static public string ObtenerTipoXMI(XElement param_elemento)
        {
            return ObtenerAtributo(param_elemento, "type");
        }

        /// <summary>
        /// Comprueba si un elemento tiene un xmi:type concreto.
        /// </summary>
        static public bool EsTipoXMI(XElement param_elemento, string param_tipo_xmi)
        {
            return ObtenerTipoXMI(param_elemento) == param_tipo_xmi;
        }

        /// <summary>
        /// Obtiene la primera referencia de un atributo XMI que puede contener uno o varios ids separados por espacios.
        /// </summary>
        static public string ObtenerPrimeraReferencia(XElement param_elemento, string param_nombre_atributo)
        {
            return ObtenerReferencias(param_elemento, param_nombre_atributo).FirstOrDefault();
        }

        /// <summary>
        /// Obtiene una referencia obligatoria. Si no existe devuelve cadena vacía.
        /// </summary>
        static public string ObtenerReferencia(XElement param_elemento, string param_nombre_atributo)
        {
            return ObtenerPrimeraReferencia(param_elemento, param_nombre_atributo) ?? string.Empty;
        }

        /// <summary>
        /// Obtiene todas las referencias de un atributo XMI separadas por espacios.
        /// </summary>
        static public List<string> ObtenerReferencias(XElement param_elemento, string param_nombre_atributo)
        {
            string valor = ObtenerAtributo(param_elemento, param_nombre_atributo);
            if (string.IsNullOrWhiteSpace(valor))
                return new List<string>();

            return valor
                .Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        /// <summary>
        /// Obtiene el valor de un hijo como lowerValue, upperValue o defaultValue.
        /// </summary>
        static public string ObtenerValorHijo(XElement param_elemento, string param_nombre_hijo)
        {
            XElement hijo = param_elemento?
                .Elements()
                .FirstOrDefault(e => e.Name.LocalName == param_nombre_hijo);

            if (hijo == null)
                return string.Empty;

            return ObtenerAtributo(hijo, "value");
        }

        /// <summary>
        /// Obtiene un atributo booleano XMI.
        /// </summary>
        static public bool ObtenerBooleano(XElement param_elemento, string param_nombre_atributo)
        {
            string valor = ObtenerAtributo(param_elemento, param_nombre_atributo);
            return valor == "true" || valor == "1";
        }

        #endregion

        #region "Transformacion de enumeraciones"

        /// <summary>
        /// Traduce un texto de XMI de tipo de asociación a la enumeración soportada por la solución.
        /// </summary>
        /// <param name="tipo_asociacion">Texto en XMI del tipo de asociación que se está procesando.</param>
        /// <returns>Enumeración con los tipos de asociaciones soportados por la solución.</returns>
        static public EnumTipoAsociacion ObtenerTipoAsociacion(string tipo_asociacion)
        {
            switch (tipo_asociacion.ToUpper())
            {
                case "UML:ASSOCIATION":
                    return EnumTipoAsociacion.ASOCIACION;
                case "SHARED":
                    return EnumTipoAsociacion.AGREGACION;
                case "COMPOSITE":
                    return EnumTipoAsociacion.COMPOSICION;
                case "GENERALIZATION":
                    return EnumTipoAsociacion.HERENCIA;
                case "UML:DEPENDENCY":
                case "UML:USAGE":
                case "UML:ABSTRACTION":
                case "UML:SUBSTITUTION":
                    return EnumTipoAsociacion.DEPENDENCIA;
                case "UML:REALIZATION":
                case "UML:INTERFACEREALIZATION":
                case "INTERFACEREALIZATION":
                    return EnumTipoAsociacion.HERENCIA;
                default:
                    return EnumTipoAsociacion.NO_DEFINIDA;
            }
        }

        /// <summary>
        /// Traduce un texto de XMI de tipo de clase a la enumeración soportada por la solución.
        /// </summary>
        /// <param name="param_tipo_clase">Texto en XMI del tipo de clase que se está procesando.</param>
        /// <param name="param_es_abstracto">True si la clase es abstracta, false si no.</param>
        /// <returns>Enumeración con los tipos de asociaciones soportados por la solución.</returns>
        static public EnumTiposClases ObtenerTipoClase(string param_tipo_clase, bool param_es_abstracto)
        {
            switch (param_tipo_clase)
            {
                case "uml:Class":
                    return param_es_abstracto ? EnumTiposClases.CLASE_ABSTRACTA : EnumTiposClases.CLASE;
                case "uml:Interface":
                    return EnumTiposClases.INTERFAZ;
                case "uml:Enumeration":
                    return EnumTiposClases.ENUMERACION;
                default:
                    return EnumTiposClases.NO_DEFINIDA;
            }
        }

        /// <summary>
        /// Traduce un texto de XMI de tipo de visibilidad a la enumeración soportada por la solución.
        /// </summary>
        /// <param name="param_visibilidad">Texto en XMI del tipo de clase que se está procesando.</param>
        /// <returns>Enumeración con los tipos de visibilidad soportados por la solución.</returns>
        static public EnumTipoVisibilidad ObtenerTipoVisibilidad(string param_visibilidad)
        {
            switch (param_visibilidad)
            {
                case "public":
                    return EnumTipoVisibilidad.PUBLICO;
                case "private":
                    return EnumTipoVisibilidad.PRIVADO;
                case "protected":
                    return EnumTipoVisibilidad.PROTEGIDO;
                case "internal":
                    return EnumTipoVisibilidad.INTERNA;
                default:
                    return EnumTipoVisibilidad.NO_DEFINIDA;
            }
        }

        /// <summary>
        /// Traduce un texto de XMI de tipo de dato a la enumeración soportada por la solución.
        /// </summary>
        /// <param name="param_tipo_dato">Texto en XMI del tipo de clase que se está procesando.</param>
        /// <returns>Enumeración con los tipos de datos soportados por la solción</returns>
        static public EnumTipoDato ObtenerTipoDato(string param_tipo_dato)
        {
            switch (param_tipo_dato)
            {
                case "pathmap://UML_LIBRARIES/UMLPrimitiveTypes.library.uml#Integer":
                    return EnumTipoDato.ENTERO;
                case "pathmap://UML_LIBRARIES/EcorePrimitiveTypes.library.uml#EFloat":
                    return EnumTipoDato.FLOTANTE;
                case "pathmap://UML_LIBRARIES/EcorePrimitiveTypes.library.uml#ELong":
                    return EnumTipoDato.LARGO;
                case "pathmap://UML_LIBRARIES/EcorePrimitiveTypes.library.uml#EShort":
                    return EnumTipoDato.CORTO;
                case "pathmap://UML_LIBRARIES/EcorePrimitiveTypes.library.uml#EDouble":
                    return EnumTipoDato.DOBLE;
                case "pathmap://UML_LIBRARIES/UMLPrimitiveTypes.library.uml#Boolean":
                    return EnumTipoDato.BOOLEANO;
                case "pathmap://UML_LIBRARIES/EcorePrimitiveTypes.library.uml#EChar":
                    return EnumTipoDato.CARACTER;
                case "pathmap://UML_LIBRARIES/UMLPrimitiveTypes.library.uml#String":
                    return EnumTipoDato.CADENA;
                case "pathmap://UML_LIBRARIES/EcorePrimitiveTypes.library.uml#EByte":
                    return EnumTipoDato.BYTE;
                default:
                    return EnumTipoDato.NO_DEFINIDO;
            }
        }
        #endregion

        #region "Procesar constructores y destructores"

        /// <summary>
        /// Recorre los constructores y los destructores para modificar los métodos asociados marcando booleanos para identificarlos.
        /// </summary>
        /// <returns>Objeto con información del proceso.</returns>
        private RetornoBase ProcesarConstructoresYDestructores()
        {
            IEnumerable<XElement> estereotipos = _documento
                .Descendants()
                .Where(e =>
                    e.Name.LocalName == "create" ||
                    e.Name.LocalName == "destroy");

            foreach (XElement estereotipo in estereotipos)
            {
                _elemento_actual = estereotipo;

                bool resultado;

                if (estereotipo.Name.LocalName == "create")
                    resultado = ProcesarConstructor(true);
                else
                    resultado = ProcesarConstructor(false);

                if (!resultado)
                {
                    string id_operacion = ObtenerAtributo(estereotipo, "base_Operation");

                    return new RetornoBase(false,
                        "No se ha podido procesar un constructor o destructor.",
                        "No se encontró la operación referenciada por base_Operation: " + id_operacion);
                }
            }

            return new RetornoBase(
                true,
                "Constructores y destructores procesados correctamente.");
        }

        /// <summary>
        /// Obtiene el identificador del método, lo busca en el proyecto y actualiza su variable sobre si es constructor o destructor.
        /// </summary>
        /// <param name="param_es_constructor">True si se está procesando un constructor, false si se está procesando un destructor.</param>
        /// <returns>True si el proceso ha sido correcto, false si no.</returns>
        protected bool ProcesarConstructor(bool param_es_constructor)
        {
            string id_metodo = ObtenerAtributo(_elemento_actual, "base_Operation");
            if (id_metodo == string.Empty)
                return false;

            Metodo metodo = BuscarComponentes.BuscarMetodo(_proyecto, id_metodo);
            if (metodo == null)
                return false;

            if (param_es_constructor)
                metodo.EsConstructor = true;
            else
                metodo.EsDestructor = true;

            return true;
        }

        #endregion
    }

    /// <summary>
    /// Clase para el control de las claves que el parser es capaz de procesar.
    /// </summary>
    static public class ClavesSoportadasXMI
    {
        /// <summary>
        /// Enumeración que define las claves soportadas en el proceso de lectura del ficheros XMI.
        /// </summary>
        public enum Enumeracion
        {
            ///<summary>Clave XMI no soportada por la solución.</summary>
            NULO = 0,

            ///<summary>Clave XMI asociada a las Clases.</summary>
            CLASE = 1,

            ///<summary>Clave XMI asociada a las asociaciones entre clases.</summary>
            ASOCIACION = 2,

            ///<summary>Clave XMI asociada a los directorios y paquetes.</summary>
            PAQUETE = 3,

            ///<summary>Clave XMI asociada a los atributos.</summary>
            ATRIBUTO = 4,

            ///<summary>Clave XMI asociada a los métodos</summary>
            METODO = 5,

            ///<summary>Clave XMI asociada a los parámetros de los métodos.</summary>
            PARAMETRO = 6,

            ///<summary>Clave XMI asociada a las herencias.</summary>
            HERENCIA = 7,

            ///<summary>Clave XMI asociada a los literales de las enumeraciones.</summary>
            LITERAL = 8,        
        }

        /// <summary>
        /// Método que devuelve el valor de la enumeración asociado a una clave.
        /// </summary>
        /// <param name="param_clave">Clave para la cual se desea obtener el valor de la enumeración.</param>
        /// <returns>Valor de la enumeración asociado a la clave.</returns>
        static public Enumeracion ObtenerEnumClave(string param_clave)
        {
            if (param_clave == "packagedElement xmi:type=\"uml:Class\"" ||
                param_clave == "packagedElement xmi:type=\"uml:Enumeration\"" ||
                param_clave == "packagedElement xmi:type=\"uml:Interface\"")
                return Enumeracion.CLASE;
            else if (param_clave == "packagedElement xmi:type=\"uml:Association\"" ||
                param_clave == "packagedElement xmi:type=\"uml:AssociationClass\"")
                return Enumeracion.ASOCIACION;
            else if (param_clave == "packagedElement xmi:type=\"uml:Dependency\"" ||
                param_clave == "packagedElement xmi:type=\"uml:Usage\"" ||
                param_clave == "packagedElement xmi:type=\"uml:Abstraction\"" ||
                param_clave == "packagedElement xmi:type=\"uml:Substitution\"" ||
                param_clave == "packagedElement xmi:type=\"uml:Realization\"" ||
                param_clave == "packagedElement xmi:type=\"uml:InterfaceRealization\"")
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
            else if (param_clave == "interfaceRealization")
                return Enumeracion.ASOCIACION;
            else if (param_clave == "ownedLiteral")
                return Enumeracion.LITERAL;
            return Enumeracion.NULO;
        }
    }
}
