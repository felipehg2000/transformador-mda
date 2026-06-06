using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using transformador_mda.src.Dominio;
using transformador_mda.src.Features.CrearIds;

namespace transformador_mda.src.Features.ParserEntrada
{
    /// <summary>
    /// Clase encargada de parsear un fichero XMI y construir un modelo de proyecto a partir de su contenido.
    /// </summary>
    public class ParserXMI : IParserEntrada
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
        /// <param name="param_directorio_raiz">Directorio raíz del proyecto.</param>
        /// <param name="param_nombre">Nombre del proyecto.</param>
        /// <param name="param_ruta_fichero">Ruta del fichero XMI.</param>
        public ParserXMI(string param_directorio_raiz, string param_nombre, string param_ruta_fichero) : base(param_ruta_fichero)
        {
            _proyecto = new ProyectoBase();
            _proyecto.DirectorioRaiz = param_directorio_raiz;
            _proyecto.Nombre = param_nombre;

            Directorio directorio = new Directorio()
            {
                Id = CrearIdentificador.CrearGUID(),
                Nombre = param_directorio_raiz,
                Namespace = param_nombre,
                Clases = new List<Clase>()
            };

            _proyecto.Directorios.Add(directorio);
        }

        #endregion

        #region "Métodos públicos"

        /// <summary>
        /// Método principal de la clase, comprueba datos e inicia el proceso de lectura.
        /// </summary>
        /// <returns>Datos del fichero XMI leído.</returns>
        /// <exception cref="Exception">Errores con el XMI que se quiere procesar.</exception>
        public override ProyectoBase Parsear()
        {
            if (string.IsNullOrEmpty(_ruta_fichero))
                throw new Exception("El fichero no puede ser vacío.");

            _documento = XDocument.Load(_ruta_fichero);

            if (_documento == null)
                throw new Exception("No se pudo cargar el documento XMI.");

            if (!EsFormatoValido())
                throw new Exception("El formato del documento XMI no es válido.");

            LeerFichero();

            return _proyecto;
        }

        #endregion

        #region "Métodos protegidos"

        /// <summary>
        /// Comprueba si el formato del fichero abierto corresponde con el formato XMI que el generador lee.
        /// </summary>
        /// <returns>True si el formato es válido, false si no.</returns>
        protected override bool EsFormatoValido()
        {
            if (_documento == null ||
                _documento.Root == null ||
                _documento.Root.Name.LocalName != "Model")
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
        protected override void LeerFichero()
        {
            foreach (var elemento in _documento.Descendants())
            {
                string clave = elemento.Name.LocalName;

                if (clave == "packagedElement")
                {
                    string tipoXmi = elemento
                        .Attributes()
                        .FirstOrDefault(a => a.Name.LocalName == "type")
                        ?.Value;

                    if (string.IsNullOrEmpty(tipoXmi))
                        continue;

                    clave += $" xmi:type=\"{tipoXmi}\"";
                }

                _clave_actual = ClavesSoportadasXMI.ObtenerEnumClave(clave);

                if (_clave_actual == ClavesSoportadasXMI.Enumeracion.NULO)
                    continue;

                _elemento_actual = elemento;
                ProcesarElemento();
            }
        }

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

            Elemento elemento = new Elemento
            {
                Id = id,
                Nombre = nombre,
                Tipo = ObtenerTipo(_elemento_actual),
                Visibilidad = EnumVisibilidad.PUBLICO,
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

                if (clasesRelacionadas.Count < 2)
                    return;

                asociacion.ClaseOrigen = clasesRelacionadas[0];
                asociacion.ClaseDestino = clasesRelacionadas[1];
            }

            _proyecto.Directorios.LastOrDefault()?.Clases.LastOrDefault()?.Asociaciones.Add(asociacion);
        }

        #endregion

        #region "Métodos privados"

        /// <summary>
        /// Obtiene el id de una línea XMI.
        /// </summary>
        /// <param name="param_elemento">Línea XMI a procesar para buscar el identificador.</param>
        /// <returns>Identificador si existe cadena vacía si no se encuentra.</returns>
        private string ObtenerId(XElement param_elemento)
        {
            return param_elemento
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == "id")
                ?.Value;
        }

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
        /// Pasado un identificador devuelve el nombre de un elemento del fichero XMI.
        /// </summary>
        /// <param name="param_id">Identificador asociado al nombre a buscar.</param>
        /// <returns>Nombre de un elemento.</returns>
        private string ObtenerNombreElementoPorId(string param_id)
        {
            if (string.IsNullOrWhiteSpace(param_id))
                return string.Empty;

            XElement elemento = _documento
                .Descendants()
                .FirstOrDefault(e => ObtenerId(e) == param_id);

            return elemento?.Attribute("name")?.Value ?? string.Empty;
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

            foreach (XElement extremo in param_elemento.Descendants())
            {
                if (extremo.Name.LocalName != "ownedEnd" &&
                    extremo.Name.LocalName != "memberEnd")
                    continue;

                string referenciaTipo = extremo.Attribute("type")?.Value;

                if (string.IsNullOrWhiteSpace(referenciaTipo))
                    referenciaTipo = extremo.Attribute("href")?.Value;

                if (!string.IsNullOrWhiteSpace(referenciaTipo) &&
                    referenciaTipo.Contains("#"))
                {
                    referenciaTipo = referenciaTipo.Substring(
                        referenciaTipo.LastIndexOf("#") + 1);
                }

                string nombreClase = ObtenerNombreElementoPorId(referenciaTipo);

                if (!string.IsNullOrWhiteSpace(nombreClase) &&
                    !ret_resultado.Contains(nombreClase))
                {
                    ret_resultado.Add(nombreClase);
                }
            }

            return ret_resultado;
        }

        #endregion
    }
}