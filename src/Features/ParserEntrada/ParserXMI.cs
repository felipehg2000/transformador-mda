using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using transformador_mda.src.Dominio;

namespace transformador_mda.src.Features.ParserEntrada
{
    public class ParserXMI : IParserEntrada
    {
        #region "Atributos"

        private ClavesSoportadasXMI.Enumeracion _clave_actual;

        private ProyectoBase _proyecto;
        
        private XDocument _documento;

        private XElement _elemento_actual;

        #endregion

        #region "Constructor"

        public ParserXMI(string param_ruta_fichero) : base(param_ruta_fichero)
        { 
            _proyecto = new ProyectoBase();
        }

        #endregion

        #region "Métodos públicos"
        
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

        protected override bool EsFormatoValido()
        {
            if (_documento == null)
                return false;

            if (_documento.Root == null)
                return false;

            if (_documento.Root.Name.LocalName != "XMI")
                return false;

            if (!_documento.Descendants().Any(e => e.Name.LocalName == "Model"))
                return false;

            bool contieneClavesSoportadas = _documento.Descendants().Any(e =>
                e.Name.LocalName == "packagedElement" ||
                e.Name.LocalName == "ownedAttribute" ||
                e.Name.LocalName == "ownedOperation" ||
                e.Name.LocalName == "ownedParameter" ||
                e.Name.LocalName == "generalization");  

            return contieneClavesSoportadas;
        }

        protected override void LeerFichero()
        {
            foreach (var elemento in _documento.Descendants())
            {
                string clave = elemento.Name.LocalName;
                if (clave == "packagedElement")
                {
                    string tipoXmi = elemento
                        .Attributes()
                        .FirstOrDefault(a => a.Name.LocalName == "type") ?.Value;

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
                Clases = new System.Collections.Generic.List<Clase>()
            };

            _proyecto.Directorios.Add(directorio);
        }

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

            Clase clase = new Clase
            {
                Id = id,
                Nombre = nombre,
                Visibilidad = Visibilidad.ObtenerEnumeracion(_elemento_actual.Attribute("visibility")?.Value),
                TipoClase = TiposClases.ObtenerEnumeracion(_elemento_actual.Attribute("xmi:type")?.Value,
                    _elemento_actual.Attribute("isAbstract")?.Value == "true"),
                Elementos = new List<Elemento>(),
                Metodos = new List<Metodo>(),
                Asociaciones = new List<Asociacion>()
            };

            _proyecto.Directorios.LastOrDefault()?.Clases.Add(clase);
        }

        protected override void ProcesarAtributoParametro()
        {
            if (_elemento_actual == null)
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
                Tipo = _elemento_actual.Attribute("type")?.Value,
                Visibilidad = Visibilidad.ObtenerEnumeracion(_elemento_actual.Attribute("visibility")?.Value),
                TipoElemento = _clave_actual == ClavesSoportadasXMI.Enumeracion.ATRIBUTO
                    ? TiposElementos.Enumeracion.ATRIBUTO
                    : TiposElementos.Enumeracion.PARAMETRO
            };

            if (elemento.TipoElemento == TiposElementos.Enumeracion.ATRIBUTO)
                _proyecto.Directorios.LastOrDefault()?.Clases.LastOrDefault()?.Elementos.Add(elemento);
            else
                _proyecto.Directorios.LastOrDefault()?.Clases.LastOrDefault()?.Metodos.LastOrDefault()?.Parametros.Add(elemento);
        }

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

            string tipoRetorno = _elemento_actual.Attribute("type")?.Value;
            if (string.IsNullOrWhiteSpace(tipoRetorno))
                tipoRetorno = "void";

            Metodo metodo = new Metodo
            {
                Id = id,
                Nombre = nombre,
                TipoRetorno = tipoRetorno,
                Visibilidad = Visibilidad.ObtenerEnumeracion(_elemento_actual.Attribute("visibility")?.Value)
            };

            _proyecto.Directorios.LastOrDefault()?.Clases.LastOrDefault()?.Metodos.Add(metodo);
        }

        protected override void ProcesarAsociacion()
        {
            if (_elemento_actual == null)
                return;

            string tipo_xmi = _elemento_actual
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == "type")
                ?.Value;

            string agregacion = _elemento_actual
                .Descendants()
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == "aggregation")
                ?.Value;

            string id = ObtenerId(_elemento_actual);
            if (string.IsNullOrEmpty(id))
                return;

            Asociacion asociacion = new Asociacion();
            if (string.IsNullOrWhiteSpace(agregacion))
                asociacion.TipoAsociacion = TiposAsociaciones.ObtenerTipoAsociacion(tipo_xmi);
            else 
                asociacion.TipoAsociacion = TiposAsociaciones.ObtenerTipoAsociacion(agregacion);

            List<string> clasesRelacionadas = ObtenerClasesRelacionadasAsociacion(_elemento_actual);
            if (clasesRelacionadas.Count < 2)
                return;

            asociacion.Id = id;
            asociacion.ClaseOrigen = clasesRelacionadas[0];
            asociacion.ClaseDestino = clasesRelacionadas[1];
            

            _proyecto.Directorios.LastOrDefault()?.Clases.LastOrDefault()?.Asociaciones.Add(asociacion);
        }
        #endregion

        #region "Métodos privados"

        List<string> ObtenerClasesRelacionadasAsociacion(XElement param_elemento)
        {
            List<string> ret_resultado = new List<string>();

            foreach(XElement extremo in param_elemento.Descendants())
            {
                if (extremo.Name.LocalName != "ownedEnd" &&
                    extremo.Name.LocalName != "memberEnd")
                    continue;

                string referenciaTipo = extremo.Attribute("type")?.Value;
                if (string.IsNullOrWhiteSpace(referenciaTipo))
                    referenciaTipo = extremo.Attribute("href")?.Value;

                string nombreClase = _proyecto.Directorios.LastOrDefault()?.ObtenerClasePorId(referenciaTipo)?.Nombre;
                if (!string.IsNullOrWhiteSpace(nombreClase) &&
                    !ret_resultado.Contains(nombreClase))
                {
                    ret_resultado.Add(nombreClase);
                }
            }

            return ret_resultado;
        }

        private string ObtenerId(XElement param_elemento)
        {
            return param_elemento
                .Attributes()
                .FirstOrDefault(a => a.Name.LocalName == "id")
                ?.Value;
        }

        #endregion
    }
}
