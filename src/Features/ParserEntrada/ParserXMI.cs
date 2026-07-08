using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Features.CrearIds;
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

            Directorio directorio = new Directorio()
            {
                Tipo = EnumTipoElemento.DIRECTORIO,
                Id = CrearIdentificador.CrearGUID(),
                Namespace = param_nombre,
                Clases = new List<Clase>()
            };

            _proyecto.Directorios.Add(directorio);
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
                return new RetornoElementoUML(false,
                    "Ruta del fichero XMI no especificada.",
                    null);

            _documento = XDocument.Load(_ruta_fichero);
            if (_documento == null)
                return new RetornoElementoUML(false,
                    "No se pudo cargar el documento XMI.",
                    null);

            if (!EsFormatoValido())
                return new RetornoElementoUML(false,
                    "El formato del documento XMI no es válido.",
                    null);

            return LeerFichero();
        }

        #endregion

        #region "Métodos protegidos"

        /// <summary>
        /// Comprueba si el formato del fichero abierto corresponde con el formato XMI que el generador lee.
        /// </summary>
        /// <returns>True si el formato es válido, false si no.</returns>
        protected override bool EsFormatoValido()
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
                e.Name.LocalName == "generalization");

            return contieneClavesSoportadas;
        }

        /// <summary>
        /// Método para leer el fichero XMI abierto. Lee línea por línea y comprueba si la clave es procesable por el generador.
        /// </summary>
        protected override RetornoElementoUML LeerFichero()
        {
            throw new NotImplementedException();

            RetornoElementoUML ret_resultado = new RetornoElementoUML();
            foreach (var elemento in _documento.Descendants())
            {
                string clave = elemento.Name.LocalName;

                if (clave == "packagedElement")
                {
                    string tipo_xmi = elemento
                        .Attributes()
                        .FirstOrDefault(a => a.Name.LocalName == "type")
                        ?.Value;

                    if (string.IsNullOrEmpty(tipo_xmi))
                        continue;

                    clave += $" xmi:type=\"{tipo_xmi}\"";
                }

                _clave_actual = ClavesSoportadasXMI.ObtenerEnumClave(clave);

                if (_clave_actual == ClavesSoportadasXMI.Enumeracion.NULO)
                    continue;

                _elemento_actual = elemento;
                ProcesarElemento();
            }
        }

        protected override RetornoElementoUML ProcesarElemento()
        {
            throw new NotImplementedException();
        }

        protected override RetornoElementoUML ProcesarDirectorio()
        {
            throw new NotImplementedException();

        }

        protected override RetornoElementoUML ProcesarClase()
        {
            throw new NotImplementedException();
        }

        protected override RetornoElementoUML ProcesarAtributo()
        {
            throw new NotImplementedException();
        }

        protected override RetornoElementoUML ProcesarMetodo()
        {
            throw new NotImplementedException();
        }

        protected override RetornoElementoUML ProcesarParametro()
        {
            throw new NotImplementedException();
        }

        protected override RetornoElementoUML ProcesarAsociacion()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region "Métodos estáticos"

        /// <summary>
        /// Obtiene el id de una línea XMI.
        /// </summary>
        /// <param name="param_elemento">Línea XMI a procesar para buscar el identificador.</param>
        /// <returns>Identificador si existe cadena vacía si no se encuentra.</returns>
        static public string ObtenerID(XElement param_elemento)
        {
            return param_elemento
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == "id")
                ?.Value;
        }

        /// <summary>
        /// Obtiene el nombre de una línea XMI.
        /// </summary>
        /// <param name="param_elemento">Línea XMI a procesar para buscar el nombre.</param>
        /// <returns>Nombre si existe cadena vacía si no se encuentra.</returns>
        static public string ObtenerNombre(XElement param_elemento)
        {
            return param_elemento
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == "name")
                ?.Value;
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
                .FirstOrDefault(e => e.Attribute("id")?.Value == param_id);
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

        #endregion

        #region "Métodos Anteriores"
        /*
        #region "Métodos protegidos"


        /// <summary>
        /// Procesa el tipo de clave que se ha leido y redirige al método que lo procesa.
        /// </summary>
        /// <exception cref="Exception">No se reconoce el elemento de la enumeración..</exception>
        protected override void ProcesarElemento()
        {
            switch (_clave_actual)
            {
                case ClavesSoportadasXMI.Enumeracion.PAQUETE:
                    ProcesarPaquete();
                    break;

                case ClavesSoportadasXMI.Enumeracion.CLASE:
                    ProcesarClase();
                    break;

                case ClavesSoportadasXMI.Enumeracion.ATRIBUTO:
                case ClavesSoportadasXMI.Enumeracion.PARAMETRO:
                    ProcesarAtributoParametro();
                    break;

                case ClavesSoportadasXMI.Enumeracion.METODO:
                    ProcesarMetodo();
                    break;

                case ClavesSoportadasXMI.Enumeracion.ASOCIACION:
                case ClavesSoportadasXMI.Enumeracion.HERENCIA:
                    ProcesarAsociacion();
                    break;

                default:
                    throw new Exception("Clave no soportada: " + _clave_actual);
            }
        }

        /// <summary>
        /// Procesa un directorio concreto.
        /// </summary>
        protected override void ProcesarPaquete()
        {
            if (_elemento_actual == null)
                return;

            string id = ObtenerId(_elemento_actual);
            if (string.IsNullOrEmpty(id))
                return;

            string nombre = _elemento_actual.Attribute("name")?.Value;
            if (string.IsNullOrWhiteSpace(nombre))
                return;

            Directorio directorio = new Directorio
            {
                Id = id,
                Nombre = nombre,
                Namespace = nombre.Replace(" ", "_").ToLower(),
                Clases = new List<Clase>()
            };

            _proyecto.Directorios.Add(directorio);
        }

        /// <summary>
        /// Procesa una clase concreta.
        /// </summary>
        protected override void ProcesarClase()
        {
            if (_elemento_actual == null)
                return;

            string id = ObtenerId(_elemento_actual);
            if (string.IsNullOrEmpty(id))
                return;

            string nombre = _elemento_actual.Attribute("name")?.Value;
            if (string.IsNullOrWhiteSpace(nombre))
                return;

            string tipoXmi = _elemento_actual
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == "type")
                ?.Value;

            Clase clase = new Clase
            {
                Id = id,
                Nombre = nombre,
                Visibilidad = EnumVisibilidad.PUBLICO,
                TipoClase = TiposClases.ObtenerEnumeracion(
                    tipoXmi,
                    _elemento_actual.Attribute("isAbstract")?.Value == "true"),
                Elementos = new List<Elemento>(),
                Metodos = new List<Metodo>(),
                Asociaciones = new List<Asociacion>()
            };

            _proyecto.Directorios.LastOrDefault()?.Clases.Add(clase);
        }

        /// <summary>
        /// Procesa un atributo o parámetro conncreto.
        /// </summary>
        protected override void ProcesarAtributoParametro()
        {
            if (_elemento_actual == null)
                return;

            if (_elemento_actual.Attribute("direction")?.Value == "return")
                return;

            string id = ObtenerId(_elemento_actual);
            if (string.IsNullOrEmpty(id))
                return;

            string nombre = _elemento_actual.Attribute("name")?.Value;
            if (string.IsNullOrEmpty(nombre))
                return;

            EnumVisibilidad visibilidad = EnumVisibilidad.PUBLICO;
            if (_clave_actual != ClavesSoportadasXMI.Enumeracion.PARAMETRO)
                visibilidad = Visibilidad.ObtenerEnumeracion(_elemento_actual.Attribute("visibility")?.Value);

            Elemento elemento = new Elemento
            {
                Id = id,
                Nombre = nombre,
                Tipo = ObtenerTipo(_elemento_actual),
                Visibilidad = visibilidad,
                TipoElemento = _clave_actual == ClavesSoportadasXMI.Enumeracion.ATRIBUTO
                    ? TiposElementos.Enumeracion.ATRIBUTO
                    : TiposElementos.Enumeracion.PARAMETRO
            };

            if (elemento.TipoElemento == TiposElementos.Enumeracion.ATRIBUTO)
                _proyecto.Directorios.LastOrDefault()?.Clases.LastOrDefault()?.Elementos.Add(elemento);
            else
                _proyecto.Directorios.LastOrDefault()?.Clases.LastOrDefault()?.Metodos.LastOrDefault()?.Parametros.Add(elemento);
        }

        /// <summary>
        /// Procesa un método concreto.
        /// </summary>
        protected override void ProcesarMetodo()
        {
            if (_elemento_actual == null)
                return;

            string id = ObtenerId(_elemento_actual);
            if (string.IsNullOrEmpty(id))
                return;

            string nombre = _elemento_actual.Attribute("name")?.Value;
            if (string.IsNullOrWhiteSpace(nombre))
                return;

            XElement parametroRetorno = _elemento_actual
                .Elements()
                .FirstOrDefault(e =>
                    e.Name.LocalName == "ownedParameter" &&
                    e.Attribute("direction")?.Value == "return");

            string tipoRetorno = parametroRetorno != null
                ? ObtenerTipo(parametroRetorno)
                : "void";

            Metodo metodo = new Metodo
            {
                Id = id,
                Nombre = nombre,
                TipoRetorno = tipoRetorno,
                Visibilidad = Visibilidad.ObtenerEnumeracion(_elemento_actual.Attribute("visibility")?.Value)
            };

            _proyecto.Directorios.LastOrDefault()?.Clases.LastOrDefault()?.Metodos.Add(metodo);
        }

        /// <summary>
        /// Procesa una asociación concreta.
        /// </summary>
        protected override void ProcesarAsociacion()
        {
            if (_elemento_actual == null)
                return;

            string tipo_xmi = _elemento_actual
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == "type")
                ?.Value;

            bool esHerencia = _elemento_actual.Name.LocalName == "generalization";

            string id = ObtenerId(_elemento_actual);
            if (string.IsNullOrEmpty(id))
                return;

            Asociacion asociacion = new Asociacion();

            if (esHerencia)
                asociacion.TipoAsociacion = TiposAsociaciones.Enumeracion.HERENCIA;
            else
                asociacion.TipoAsociacion = TiposAsociaciones.ObtenerTipoAsociacion(tipo_xmi);

            asociacion.Id = id;

            if (esHerencia)
            {
                asociacion.ClaseOrigen = _elemento_actual.Attribute("general")?.Value;
                asociacion.ClaseDestino = _proyecto.Directorios.LastOrDefault()?.Clases.LastOrDefault()?.Id;
            }
            else
            {
                List<string> clasesRelacionadas = ObtenerClasesRelacionadasAsociacion(_elemento_actual);

                //if (clasesRelacionadas.Count < 2)
                //    return;

                if (clasesRelacionadas.Count == 1)
                {
                    asociacion.ClaseDestino = clasesRelacionadas[0];
                }
                else
                {
                    asociacion.ClaseOrigen = clasesRelacionadas[0];
                    asociacion.ClaseDestino = clasesRelacionadas[1];
                }
            }

            _proyecto.Directorios.LastOrDefault()?.Clases.LastOrDefault()?.Asociaciones.Add(asociacion);
        }

        #endregion

        #region "Métodos privados"

        /// <summary>
        /// Obtiene el tipo de atributo o parámetro del elemento que se está procesando.
        /// </summary>
        /// <param name="param_elemento">Elemento que se está procesando.</param>
        /// <returns>Tipado del elemento.</returns>
        private string ObtenerTipo(XElement param_elemento)
        {
            if (param_elemento == null)
                return "object";

            string tipo = param_elemento.Attribute("type")?.Value;

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                string nombreTipo = ObtenerNombreElementoPorId(tipo);

                if (!string.IsNullOrWhiteSpace(nombreTipo))
                    return NormalizarTipoCSharp(nombreTipo);

                return NormalizarTipoCSharp(tipo);
            }

            XElement nodoTipo = param_elemento
                .Elements()
                .FirstOrDefault(e => e.Name.LocalName == "type");

            string href = nodoTipo?.Attribute("href")?.Value;

            if (!string.IsNullOrWhiteSpace(href))
            {
                int posicionAlmohadilla = href.LastIndexOf('#');

                if (posicionAlmohadilla >= 0)
                    href = href.Substring(posicionAlmohadilla + 1);

                return NormalizarTipoCSharp(href);
            }

            return "object";
        }

        /// <summary>
        /// Según los tipados que devuelve el fichero XMI, transformarlos en tipos adecuados para C#
        /// </summary>
        /// <param name="param_tipo"></param>
        /// <returns></returns>
        private string NormalizarTipoCSharp(string param_tipo)
        {
            if (string.IsNullOrWhiteSpace(param_tipo))
                return "object";

            switch (param_tipo.ToLower())
            {
                case "string":
                    return "string";

                case "integer":
                case "int":
                    return "int";

                case "boolean":
                case "bool":
                    return "bool";

                case "float":
                    return "float";

                case "double":
                    return "double";

                case "decimal":
                    return "decimal";

                case "long":
                    return "long";

                case "short":
                    return "short";

                case "byte":
                    return "byte";

                case "char":
                    return "char";

                default:
                    return param_tipo;
            }
        }

        /// <summary>
        /// Devuelve las clases que están relacionadas por una asociación.
        /// </summary>
        /// <param name="param_elemento">Elemento de asociación a procesar.</param>
        /// <returns>Lista de strings con los nombres de las clases asociadas.</returns>
        private List<string> ObtenerClasesRelacionadasAsociacion(XElement param_elemento)
        {
            List<string> ret_resultado = new List<string>();

            if (param_elemento == null)
                return ret_resultado;

            string memberEnd = param_elemento.Attribute("memberEnd")?.Value;

            if (!string.IsNullOrWhiteSpace(memberEnd))
            {
                string[] idsExtremos = memberEnd.Split(
                    new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (string idExtremo in idsExtremos)
                {
                    XElement extremo = _documento
                        .Descendants()
                        .FirstOrDefault(e => ObtenerId(e) == idExtremo);

                    if (extremo == null)
                        continue;

                    string idClase = ObtenerIdClaseDesdeExtremo(extremo);

                    if (!string.IsNullOrWhiteSpace(idClase) &&
                        !ret_resultado.Contains(idClase))
                    {
                        ret_resultado.Add(idClase);
                    }
                }
            }

            foreach (XElement extremo in param_elemento.Descendants())
            {
                if (extremo.Name.LocalName != "ownedEnd")
                    continue;

                string idClase = ObtenerIdClaseDesdeExtremo(extremo);

                if (!string.IsNullOrWhiteSpace(idClase) &&
                    !ret_resultado.Contains(idClase))
                {
                    ret_resultado.Add(idClase);
                }
            }

            return ret_resultado;
        }

        /// <summary>
        /// Método encargado de obtener los ids de dos clases según su asociación
        /// </summary>
        /// <param name="param_extremo">Elemento del que obtener el identificador.</param>
        /// <returns>Listado de los identificadores asociados a una relación.</returns>
        private string ObtenerIdClaseDesdeExtremo(XElement param_extremo)
        {
            if (param_extremo == null)
                return string.Empty;

            string referenciaTipo = param_extremo.Attribute("type")?.Value;

            if (string.IsNullOrWhiteSpace(referenciaTipo))
                referenciaTipo = param_extremo.Attribute("href")?.Value;

            if (!string.IsNullOrWhiteSpace(referenciaTipo) &&
                referenciaTipo.Contains("#"))
            {
                referenciaTipo = referenciaTipo.Substring(
                    referenciaTipo.LastIndexOf("#") + 1);
            }

            return referenciaTipo ?? string.Empty;
        }

        #endregion
        */
        #endregion "Métodos Anteriores"
    }

    static public class ClavesSoportadasXMI
    {

        /// <summary>
        /// Enumeración que define las claves soportadas en el proceso de lectura del ficheros XMI.
        /// </summary>
        public enum Enumeracion
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
        static public string ObtenerClave(Enumeracion param_enum_clave)
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