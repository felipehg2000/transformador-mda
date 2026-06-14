using System.IO;
using System.Collections.Generic;
using transformador_mda.src.Dominio;
using transformador_mda.src.Features.GenerarSalida.Plantillas;

namespace transformador_mda.src.Features.GenerarSalida
{
    /// <summary>
    /// Clase encargada de generar los artefactos software y las definiciones de los métodos y atributos para la salida C Sharp.
    /// </summary>
    public class C_Sharp : GenerarSalidaBase
    {
        #region "Constructor"

        /// <summary>
        /// Constructor de la clase, inicializa atributos.
        /// </summary>
        /// <param name="param_proyecto">Datos del proyecto para generar la salida.</param>
        public C_Sharp(ProyectoBase param_proyecto) : base(param_proyecto)
        {

        }

        #endregion

        #region "Métodos públicos"

        /// <summary>
        /// Método principal de la clase, se encarga de comprobar que los datos rellenos sean correctos y llama al método que genera la salida.
        /// </summary>
        /// <returns>True si el proceso ha sido correcto, false si ha ocurrido algún error.</returns>
        public override bool GenerarSalida()
        {
            if (_proyecto == null)
                return false;

            if (string.IsNullOrWhiteSpace(_proyecto.DirectorioRaiz))
                return false;

            foreach (var directorio in _proyecto.Directorios)
            {
                if (directorio == null)
                    continue;

                if (directorio.Clases == null || directorio.Clases.Count == 0)
                    continue;

                _directorio_actual = directorio;
                GenerarClases();
            }

            return true;
        }

        #endregion

        #region "Métodos protegidos"

        /// <summary>
        /// Recorre todas las clases incluidas en un directorio y las genera.
        /// </summary>
        protected override void GenerarClases()
        {
            if (_directorio_actual == null)
                return;

            if (_directorio_actual.Clases == null || _directorio_actual.Clases.Count == 0)
                return;

            string ruta_directorio = _proyecto.DirectorioRaiz;

            if (!string.IsNullOrWhiteSpace(_directorio_actual.Nombre) &&
                _directorio_actual.Nombre != _proyecto.DirectorioRaiz)
            {
                ruta_directorio = Path.Combine(_proyecto.DirectorioRaiz, _directorio_actual.Nombre);
            }

            Directory.CreateDirectory(ruta_directorio);

            foreach (var clase in _directorio_actual.Clases)
            {
                if (clase == null || string.IsNullOrWhiteSpace(clase.Nombre))
                    continue;

                _clase_actual = clase;

                string contenido = GenerarCodigoClase();

                string ruta_fichero = Path.Combine(
                    ruta_directorio,
                    clase.Nombre + ".cs");

                File.WriteAllText(ruta_fichero, contenido);
            }
        }

        /// <summary>
        /// Genera el código necesario para que la clase tenga todas las definiciones hechas en el diagrama de clases.
        /// </summary>
        /// <returns>Código de la clase.</returns>
        protected override string GenerarCodigoClase()
        {
            if (_clase_actual == null)
                return string.Empty;

            string atributos = GenerarElementos(
                _clase_actual.Elementos.FindAll(e =>
                    e.TipoElemento != TiposElementos.Enumeracion.PARAMETRO &&
                    e.Visibilidad == EnumVisibilidad.PUBLICO));

            string propiedades = GenerarElementos(
                _clase_actual.Elementos.FindAll(e =>
                    e.TipoElemento != TiposElementos.Enumeracion.PARAMETRO &&
                    e.Visibilidad != EnumVisibilidad.PUBLICO));

            string metodosPublicos = GenerarMetodos(
                _clase_actual.Metodos.FindAll(m => m.Visibilidad == EnumVisibilidad.PUBLICO));

            string metodosProtegidos = GenerarMetodos(
                _clase_actual.Metodos.FindAll(m => m.Visibilidad == EnumVisibilidad.PROTEGIDO));

            string metodosPrivados = GenerarMetodos(
                _clase_actual.Metodos.FindAll(m => m.Visibilidad == EnumVisibilidad.PRIVADO));

            string textoNamespace = _proyecto.Nombre + "." + _directorio_actual.Namespace;

            string herencia = GenerarHerencia();

            string ret_texto = PlantillasC_Sharp.ClaseBase;

            ret_texto = ret_texto.Replace("{Relaciones}", GenerarReferencias());
            ret_texto = ret_texto.Replace("{Namespace}", textoNamespace);
            ret_texto = ret_texto.Replace("{Visibilidad}", Visibilidad.ObtenerVisibilidad(_clase_actual.Visibilidad));
            ret_texto = ret_texto.Replace("{TipoClase}", ObtenerTextoTipoClase(_clase_actual));
            ret_texto = ret_texto.Replace("{NombreClase}", _clase_actual.Nombre);
            ret_texto = ret_texto.Replace("{Herencia}", herencia);
            ret_texto = ret_texto.Replace("{Atributos}", atributos);
            ret_texto = ret_texto.Replace("{Propiedades}", propiedades);
            ret_texto = ret_texto.Replace("{MetodosPublicos}", metodosPublicos);
            ret_texto = ret_texto.Replace("{MetodosProtegidos}", metodosProtegidos);
            ret_texto = ret_texto.Replace("{MetodosPrivados}", metodosPrivados);

            return ret_texto;
        }

        /// <summary>
        /// Genera el código para crear atributos y parámetros.
        /// </summary>
        /// <remarks>
        /// Si el atributo es público se trata como propiedad.
        /// </remarks>
        /// <param name="param_elementos">Listado de elementos cuyo código hay que generar.</param>
        /// <returns>Código de los elementos pasados por parámetros.</returns>
        protected override string GenerarElementos(List<Elemento> param_elementos)
        {
            if (param_elementos == null || param_elementos.Count == 0)
                return string.Empty;

            string ret_texto = string.Empty;

            foreach (var elemento in param_elementos)
            {
                if (elemento == null)
                    continue;

                if (elemento.TipoElemento == TiposElementos.Enumeracion.PARAMETRO)
                    continue;

                if (elemento.Visibilidad == EnumVisibilidad.PUBLICO)
                {
                    ret_texto += "\t\t\tpublic " + elemento.Tipo + " " + elemento.Nombre + " { get; set; }\n";
                }
                else
                {
                    ret_texto += "\t\t\t" +
                        Visibilidad.ObtenerVisibilidad(elemento.Visibilidad) + " " +
                        elemento.Tipo + " " +
                        elemento.Nombre + ";\n";
                }
            }

            return ret_texto;
        }

        /// <summary>
        /// Genera el código de los métodos que haya definidos en el diagrama de clases.
        /// </summary>
        /// <param name="param_metodos">Listado de métodos a generar.</param>
        /// <returns>Código con los métodos pasados por parámetros.</returns>
        protected override string GenerarMetodos(List<Metodo> param_metodos)
        {
            if (param_metodos == null || param_metodos.Count == 0)
                return string.Empty;

            string ret_texto = string.Empty;

            foreach (var metodo in param_metodos)
            {
                if (metodo == null)
                    continue;

                string textoMetodo = PlantillasC_Sharp.Metodo;

                textoMetodo = textoMetodo.Replace("{Visibilidad}", Visibilidad.ObtenerVisibilidad(metodo.Visibilidad));
                textoMetodo = textoMetodo.Replace("{TipoRetorno}", metodo.TipoRetorno.Trim());
                textoMetodo = textoMetodo.Replace("{NombreMetodo}", metodo.Nombre);
                textoMetodo = textoMetodo.Replace("{Parametros}", GenerarParametros(metodo.Parametros));

                ret_texto += textoMetodo + "\n\n";
            }

            return ret_texto;
        }

        #endregion

        #region "Métodos privados"

        /// <summary>
        /// Genera el código para los parámetros de un método.
        /// </summary>
        /// <param name="param_parametros">Listado de parámetros de un método.</param>
        /// <returns>Código de los parámetros pasados por parámetro.</returns>
        private string GenerarParametros(List<Elemento> param_parametros)
        {
            if (param_parametros == null || param_parametros.Count == 0)
                return string.Empty;

            List<string> parametros = new List<string>();

            foreach (var parametro in param_parametros)
            {
                if (parametro == null)
                    continue;

                if (parametro.TipoElemento != TiposElementos.Enumeracion.PARAMETRO)
                    continue;

                parametros.Add(parametro.Tipo + " " + parametro.Nombre);
            }

            return string.Join(", ", parametros);
        }

        /// <summary>
        /// Genera la herencia de una clase para la definición de la clase.
        /// </summary>
        /// <returns>Código de la dependencia de una clase hijo sobre los datos de su clase padre.</returns>
        private string GenerarHerencia()
        {
            if (_clase_actual == null || _clase_actual.Asociaciones == null)
                return string.Empty;

            Asociacion asociacion_tmp = _clase_actual.Asociaciones
                .Find(a => a.TipoAsociacion == TiposAsociaciones.Enumeracion.HERENCIA);

            if (asociacion_tmp == null)
                return string.Empty;

            string id_clase_origen = asociacion_tmp.ClaseOrigen;

            foreach (var directorio in _proyecto.Directorios)
            {
                if (directorio.Clases == null)
                    continue;

                Clase clasePadre = directorio.Clases.Find(c => c.Id == id_clase_origen);

                if (clasePadre != null)
                    return " : " + clasePadre.Nombre;
            }

            return string.Empty;
        }

        /// <summary>
        /// Casteo del tipo de clase que viene en el XMI al texto de la clase que hay que poner en el código.
        /// </summary>
        /// <param name="param_clase">Tipo de clase en lenguaje XMI.</param>
        /// <returns>Tipo de clase en lenguaje C#</returns>
        private string ObtenerTextoTipoClase(Clase param_clase)
        {
            string tipo = TiposClases.ObtenerTexto(param_clase.TipoClase);

            if (tipo == "uml:Class")
                return "class";

            if (tipo == "uml:Interface")
                return "interface";

            if (tipo == "uml:Enumeration")
                return "enum";

            return tipo;
        }

        /// <summary>
        /// Genera los using necesarios para las asociaciones de la clase.
        /// </summary>
        /// <returns>Texto con los using.</returns>
        private string GenerarReferencias()
        {
            if (_clase_actual == null ||
                _clase_actual.Asociaciones == null ||
                _clase_actual.Asociaciones.Count == 0)
            {
                return string.Empty;
            }

            List<string> referencias = new List<string>();

            foreach (Asociacion asociacion in _clase_actual.Asociaciones)
            {
                string idClaseRelacionada;

                if (asociacion.ClaseOrigen == _clase_actual.Id)
                    idClaseRelacionada = asociacion.ClaseDestino;
                else
                    idClaseRelacionada = asociacion.ClaseOrigen;

                Directorio directorioClaseRelacionada = null;

                foreach (Directorio directorio in _proyecto.Directorios)
                {
                    if (directorio.Clases.Find(c => c.Id == idClaseRelacionada) != null)
                    {
                        directorioClaseRelacionada = directorio;
                        break;
                    }
                }

                if (directorioClaseRelacionada == null)
                    continue;

                if (directorioClaseRelacionada.Namespace == _directorio_actual.Namespace)
                    continue;

                string referencia = "using " + _proyecto.Nombre + "." + directorioClaseRelacionada.Namespace + ";";

                if (!referencias.Contains(referencia))
                    referencias.Add(referencia);
            }

            return string.Join("\n", referencias);
        }

        #endregion
    }
}