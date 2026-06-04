using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using transformador_mda.src.Dominio;
using transformador_mda.src.Features.GenerarSalida.Plantillas;

namespace transformador_mda.src.Features.GenerarSalida
{
    public class C_Sharp : GenerarSalidaBase
    {
        #region "Constructor"

        /// <summary>
        /// Constructor de la clase, inicializa atributos.
        /// </summary>
        /// <param name="param_proyecto">Proyecto del cual se generará la salida.</param>
        public C_Sharp(ProyectoBase param_proyecto) : base(param_proyecto)
        {

        }

        #endregion

        #region "Métodos públicos"

        public override bool GenerarSalida()
        {
            if (_proyecto == null)
                return false;

            foreach (var directorio in _proyecto.Directorios)
            {
                if (directorio == null)
                    continue;

                if (directorio.Clases.Count != 0)
                {
                    _directorio_actual = directorio;
                    GenerarClases();
                }
            }

            return true;
        }

        #endregion

        #region "Métodos protegidos

        protected override void GenerarClases()
        {
            string ruta_directorio = Path.Combine(_proyecto.DirectorioRaiz, _directorio_actual.Nombre);

            if (string.IsNullOrEmpty(ruta_directorio) ||
                _directorio_actual.Clases == null ||
                _directorio_actual.Clases.Count == 0)
                return;

            foreach (var clase in _directorio_actual.Clases)
            {
                if (clase == null || string.IsNullOrEmpty(clase.Nombre))
                    continue;

                _clase_actual = clase;
                string contenido = GenerarCodigoClase();

                string ruta_fichero = Path.Combine(ruta_directorio, clase.Nombre + ".cs");
                File.WriteAllText(ruta_fichero, contenido);
            }
        }

        protected override string GenerarCodigoClase()
        {
            if (_clase_actual == null)
                return string.Empty;

            string atributos = GenerarElementos(_clase_actual.Elementos.FindAll(e => e.Visibilidad == EnumVisibilidad.PUBLICO));
            string propiedades = GenerarElementos(_clase_actual.Elementos.FindAll(e => e.Visibilidad != EnumVisibilidad.PUBLICO));
            string constructores = string.Empty;

            string metodosPublicos = GenerarMetodos(_clase_actual.Metodos.FindAll(m => m.Visibilidad == EnumVisibilidad.PUBLICO));
            string metodosInternos = GenerarMetodos(_clase_actual.Metodos.FindAll(m => m.Visibilidad == EnumVisibilidad.INTERNA));
            string metodosPrivados = GenerarMetodos(_clase_actual.Metodos.FindAll(m => m.Visibilidad == EnumVisibilidad.PROTEGIDO));
            string metodosProtegidos = GenerarMetodos(_clase_actual.Metodos.FindAll(m => m.Visibilidad == EnumVisibilidad.PRIVADO));

            string textoNmaespace = Path.Combine(_directorio_actual.Namespace, _clase_actual.Nombre);

            string herencia = string.Empty;
            Asociacion asociacion_tmp = _clase_actual.Asociaciones.Find(a => a.TipoAsociacion == TiposAsociaciones.Enumeracion.HERENCIA);
            if (asociacion_tmp != null)
            {
                string id_clase_origen = asociacion_tmp.ClaseOrigen;

                foreach (var directorio in _proyecto.Directorios)
                {
                    Clase tmp = directorio.Clases.Find(c => c.Id == id_clase_origen);
                    if (tmp.Id == id_clase_origen)
                    {
                        herencia = ": " + tmp.Nombre;
                        break;
                    }
                }
            }

            string ret_texto = PlantillasC_Sharp.ClaseBase;

            ret_texto = ret_texto.Replace("{Namespace}", textoNmaespace);
            ret_texto = ret_texto.Replace("{Visibilidad}", Visibilidad.ObtenerVisibilidad(_clase_actual.Visibilidad));
            ret_texto = ret_texto.Replace("{TipoClase}", TiposClases.ObtenerTexto(_clase_actual.TipoClase));
            ret_texto = ret_texto.Replace("{NombreClase}", _clase_actual.Nombre);
            ret_texto = ret_texto.Replace("{Herencia}"          , herencia);
            ret_texto = ret_texto.Replace("{Atributos}"         , atributos);
            ret_texto = ret_texto.Replace("{Propiedades}"       , propiedades);
            ret_texto = ret_texto.Replace("{Constructores}"     , constructores);
            ret_texto = ret_texto.Replace("{MetodosPublicos}"   , metodosPublicos);
            ret_texto = ret_texto.Replace("{MetodosProtegidos}" , metodosProtegidos);
            ret_texto = ret_texto.Replace("{MetodosPrivados}"   , metodosPrivados);

            return ret_texto;
        }

        protected override string GenerarElementos(List<Elemento> param_elementos)
        {
            if (param_elementos.Count == 0)
                return string.Empty;

            string ret_texto = string.Empty;
            foreach (var elemento in param_elementos)
            {
                if (elemento.TipoElemento == TiposElementos.Enumeracion.PARAMETRO)
                    ret_texto += elemento.TipoElemento + " " + elemento.Nombre + ",";
                if (elemento.Visibilidad == EnumVisibilidad.PUBLICO)
                    ret_texto += "\t\t\tpublic " + elemento.TipoElemento + " " + elemento.Nombre + " { get; set; }\n";
                else
                    ret_texto += "\t\t\t" + Visibilidad.ObtenerVisibilidad(elemento.Visibilidad) + " " + elemento.TipoElemento + " " + elemento.Nombre + ";\n";
            }

            if (param_elementos.ElementAt(0).TipoElemento == TiposElementos.Enumeracion.PARAMETRO)
                ret_texto.TrimEnd(',');

            return ret_texto;
        }

        protected override string GenerarMetodos(List<Metodo> param_metodos)
        {
            string ret_texto = string.Empty;

            if (param_metodos.Count > 0)
                return ret_texto;

            foreach (var metodo in param_metodos)
            {
                string textoMetodo = PlantillasC_Sharp.Metodo;
                textoMetodo = textoMetodo.Replace("{Visibilidad}"   , Visibilidad.ObtenerVisibilidad(metodo.Visibilidad));
                textoMetodo = textoMetodo.Replace("{TipoRetorno}"   , metodo.TipoRetorno);
                textoMetodo = textoMetodo.Replace("{NombreMetodo}"  , metodo.Nombre);
                textoMetodo = textoMetodo.Replace("{Parametros}"    , GenerarElementos(metodo.Parametros));
                textoMetodo = textoMetodo += "\n\n";

                ret_texto += textoMetodo;
            }

            return ret_texto;
        }

        #endregion
    }
}
