using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Features.CrearIds;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static transformador_mda.src.Features.ParserEntrada.ClavesSoportadasXMI;

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
                e.Name.LocalName == "generalization");

            return contieneClavesSoportadas;
        }

        /// <summary>
        /// Método para leer el fichero XMI, busca los elementos principales y busca su cierre para procesar el contenido de este.
        /// </summary>
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
                   // retorno = ProcesarAsociacion();
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
            }

            return new RetornoElementoUML() { Exito = true, Elemento = nuevo_nodo };
        }

        protected RetornoElementoUML ProcesarClase()
        {
            EnumTiposClases tipo_clase = EnumTiposClases.NO_DEFINIDA;
            string clave_xmi = ObtenerClaveXMI(_elemento_actual);
            if (clave_xmi == "packagedElement xmi:type=\"uml:Class\"")
                tipo_clase = EnumTiposClases.CLASE;
            else if (clave_xmi == "packagedElement xmi:type=\"uml:Enumeration\"")
                tipo_clase = EnumTiposClases.ENUMERACION;
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
                    nuevo_nodo.TipoRetorno = ((Parametro)resultado.Elemento).TipoDato;
            }

            return new RetornoElementoUML() { Exito = true, Elemento = nuevo_nodo };
        }

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

        protected RetornoElementoUML ProcesarAsociacion()
        {
            throw new NotImplementedException();
        }

        protected RetornoElementoUML ProcesarHerencia()
        {
            throw new NotImplementedException();
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

        static public string ObtenerComentario(XElement param_elemento)
        {
            if (param_elemento == null)
                return string.Empty;

            XElement comentario = param_elemento.Elements().FirstOrDefault(e => e.Name == "ownedComment");
            if (comentario == null)
                return string.Empty;

            XElement cuerpo = param_elemento.Elements().FirstOrDefault(c => c.Name == "body");
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

        static public EnumTipoVisibilidad ObtenerVisibilidad(XElement param_elemento)
        {
            string visibilidad = param_elemento.Attribute("visibility")?.Value;
            if (string.IsNullOrEmpty(visibilidad))
                return EnumTipoVisibilidad.NO_DEFINIDA;

            return ObtenerTipoVisibilidad(visibilidad);
        }

        static public EnumTipoDato ObtenerTipoDeDato(XElement param_elemento)
        {
            string tipo_dato = param_elemento.Attribute("href")?.Value;
            if (string.IsNullOrEmpty(tipo_dato))
                return EnumTipoDato.NO_DEFINIDO;

            return ObtenerTipoDato(tipo_dato);
        }

        #endregion

        #region "Transformacion de enumeraciones"
        
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
                    return EnumTipoAsociacion.DEPENDENCIA;
                default:
                    return EnumTipoAsociacion.NO_DEFINIDA;
            }
        }

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
            LITERAL = 8
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
                case Enumeracion.LITERAL:
                    return "ownedLiteral";
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
            if (param_clave == "packagedElement xmi:type=\"uml:Class\"" ||
                param_clave == "packagedElement xmi:type=\"uml:Enumeration\"" ||
                param_clave == "packagedElement xmi:type=\"uml:Interface\"")
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
            else if (param_clave == "ownedLiteral")
                return Enumeracion.LITERAL;
            return Enumeracion.NULO;
        }
    }
}