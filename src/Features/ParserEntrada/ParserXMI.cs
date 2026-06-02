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

        private XDocument _documento;

        private XElement _elemento_actual;

        private ClavesSoportadasXMI.Enumeracion _clave_actual;

        private ProyectoBase _proyecto;

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
            

            return new ProyectoBase(); // Aquí deberías devolver el proyecto parseado
        }

        #endregion

        #region "Métodos privados"

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
                case ClavesSoportadasXMI.Enumeracion.HERENCIA:
                    ProcesarHerencia();
                    break;
                case ClavesSoportadasXMI.Enumeracion.ASOCIACION:
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

            string nombre = _elemento_actual.Attribute("name")?.Value;

            if (string.IsNullOrWhiteSpace(nombre))
                return;

            Directorio directorio = new Directorio
            {
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

            string nombre = _elemento_actual.Attribute("name")?.Value;

            if (string.IsNullOrWhiteSpace(nombre))
                return;

            Clase clase = new Clase
            {
                Nombre = nombre,
                Visibilidad = Visibilidad.ObtenerEnumeracion(_elemento_actual.Attribute("visibility")?.Value),
                TipoClase = TipoClase.ObtenerEnumeracion(_elemento_actual.Attribute("xmi:type")?.Value,
                    _elemento_actual.Attribute("isAbstract")?.Value == "true"),
                Elementos = new List<Elemento>(),
                Metodos = new List<Metodo>(),
                Asociaciones = new List<Asociacion>()
            };

            _proyecto.Directorios.LastOrDefault()?.Clases.Add(clase);
        }

        protected override void ProcesarAtributoParametro()
        {

        }

        protected override void ProcesarMetodo()
        {

        }

        protected override void ProcesarHerencia()
        {

        }

        protected override void ProcesarAsociacion()
        {

        }

        #endregion
    }
}
