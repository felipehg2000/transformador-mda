/**
 Propagación de errores en Principal.GenerarCodigo. Se hará cuando se termine el generador
Orientación y navegabilidad de asociaciones/dependencias. Creo que está solucionado
Generación de enumeraciones. Solucionado
Tratamiento específico de interfaces. Pendiente
Orden de clase base e interfaces. Solucionado
Comprobación del retorno de GenerarNamespace. Pendiente
Representación explícita de constructores. Pendiente
Sustitución de concatenaciones y Contains por colecciones y string.Join. Pendiente
 */

using System;
using System.Collections.Generic;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Features.Utilidades;
using transformador_mda.src.Dominio.ElementosUML;

namespace transformador_mda.src.Features.GenerarSalida
{
    /// <summary>
    /// Clase encargada de generar los artefactos software y las definiciones de los métodos y atributos para la salida C Sharp.
    /// </summary>
    public class C_Sharp : GenerarSalidaBase
    {
        #region "Constructor"

        /// <summary>
        /// Constructor de la clase, inicializa variables.
        /// </summary>
        /// <param name="param_proyecto">Información del proyecto que la aplicación está gestionando.</param>
        public C_Sharp(ProyectoBase param_proyecto) : 
            base(param_proyecto)
        {

        }

        #endregion

        #region "Métodos publicos heredados"

        /// <summary>
        /// Método sin implementar
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override RetornoBase GenerarSalida()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Genera el código de una única clase.
        /// </summary>
        /// <param name="param_clase">Objeto de tipo clase del cual se generará el código.</param>
        /// <returns>Objeto con el código generado e información del proceso.</returns>
        public override RetornoBase GenerarTexto(Clase param_clase)
        {
            if (param_clase == null || _proyecto == null)
                return new RetornoBase(false, 
                    "No se ha podido generar el código correctamente.", 
                    "En el método GenerarTexto de C_Sharp el parámetro param_clase es nulo.");

            return GenerarCodigoClases(_proyecto, param_clase);
        }

        #endregion

        #region "Generar código de elemento"

        /// <summary>
        /// Genera el código de los atributos de una clase.
        /// </summary>
        /// <param name="param_atributos">Lista de atributos asociados a una clase.</param>
        /// <returns>Objecto con el código de una clase e información del proceso de generación..</returns>
        static private RetornoBase GenerarCodigoAtributos(List<Atributo> param_atributos)
        {
            if (param_atributos.Count <= 0)
                return new RetornoBase(true, "Código generado correctamente", "");

            string texto_atributos = "\t\t#region \"Atributos\"\n\n";

            foreach (Atributo atributo in param_atributos)
            {
                texto_atributos += "\t\t/// <summary>\n";
                texto_atributos += "\t\t/// " + atributo.Comentario + "\n";
                texto_atributos += "\t\t/// </summary>\n";
                texto_atributos += "\t\t" + ObtenerTipoVisibilidad(atributo.Visibilidad) + " " + ObtenerTipoDato(atributo.TipoDato) + " " + atributo.Nombre;
                if (atributo.Visibilidad == EnumTipoVisibilidad.PUBLICO)
                    texto_atributos += " { get; set; } \n\n";
                else
                    texto_atributos += ";\n\n";

            }             

            texto_atributos += "\t\t#endregion";

            return new RetornoBase(true, 
                "Código generado correctamente.", 
                texto_atributos);
        }

        /// <summary>
        /// Genera el código de una clase diferenciando por su tipo.
        /// </summary>
        /// <param name="param_proyecto">Datos de todo el proyecto.</param>
        /// <param name="param_clase">Datos de la clase cuyo código debe generarse.</param>
        /// <returns>Objecto con el código de una clase e información del proceso de generación.</returns>
        static private RetornoBase GenerarCodigoClases(ProyectoBase param_proyecto, Clase param_clase)
        {
            RetornoBase retorno_herencia = GenerarHerencia(param_proyecto, param_clase.Id, param_clase.Asociaciones);
            if (!retorno_herencia.Exito)
                return retorno_herencia;

            string[] partes = retorno_herencia.TextoDesarrollador.Split(new string[] { "--{{texto_separacion}}--" }, StringSplitOptions.None);
            string texto_librerias = partes.Length > 0 ? partes[0] : string.Empty;
            string texto_herencia = partes.Length > 1 ? partes[1] : string.Empty;

            RetornoBase retorno_librerias = GenerarAsociaciones(param_proyecto, param_clase.Id, param_clase.Asociaciones);
            if (!retorno_librerias.Exito)
                return retorno_librerias;

            if (retorno_librerias.TextoDesarrollador.Contains(texto_librerias))
                texto_librerias = retorno_librerias.TextoDesarrollador;
            else
                texto_librerias += retorno_librerias.TextoDesarrollador;

            RetornoBase retorno_namespace = GenerarNamespace(param_proyecto, param_clase);
            if (!retorno_namespace.Exito)
                return retorno_namespace;

            string texto_clases = texto_librerias + "\n" +
                "namespace " + retorno_namespace.TextoDesarrollador + "\n" +
                "{\n" +
                "\t" + ObtenerTipoVisibilidad(param_clase.Visibilidad) + " " + ObtenerTipoClase(param_clase.TipoClase) + " " + param_clase.Nombre;

            texto_clases += texto_herencia != string.Empty ? ": " + texto_herencia + "\n" : "\n"; 
            texto_clases += "\t{\n";


            if (param_clase.TipoClase == EnumTiposClases.ENUMERACION)
            {
                texto_clases += GenerarCodigoLiterales(param_clase.Literales).TextoDesarrollador +
                    "\t}\n" +
                    "}\n\n";
                
                return new RetornoBase(true, "Código generado correctamente.", texto_clases);
            }

            RetornoBase retorno = GenerarCodigoAtributos(param_clase.Atributos);
            if (!retorno.Exito)
                return retorno;

            texto_clases += retorno.TextoDesarrollador;

            retorno = GenerarCodigoMetodo(param_clase.Metodos);
            if (!retorno.Exito)
                return retorno;

            texto_clases += retorno.TextoDesarrollador + "\n" +
                    "\t}\n" +
                    "}\n\n";

            return new RetornoBase(true, "Código generado correctamente.", texto_clases);            
        }

        //static private RetornoBase GenerarCodigoInterfaz();

        //static private RetornoBase GenerarCodigoClase()

        //static private RetornoBase GenerarCodigoClaseAbstracta();

        /// <summary>
        /// Genera la definición de un método con sus comentarios y definición.
        /// </summary>
        /// <param name="param_metodos">Objeto con los datos del método a generar.</param>
        /// <returns>Objeto con el código de un método e información del proceso de generación.</returns>
        static private RetornoBase GenerarCodigoMetodo(List<Metodo> param_metodos)
        {
            if (param_metodos.Count <= 0)
                return new RetornoBase(true, "Código generado correctamente.", "");

            string texto_constructores = string.Empty;
            string texto_metodos_privades = string.Empty;
            string texto_metodos_publicos = string.Empty;
            string texto_metodos_protegidos = string.Empty;
            string texto_metodos_internos = string.Empty;
            foreach (Metodo metodo in param_metodos)
            {
                RetornoBase retorno = GenerarCodigoParametro(metodo.Parametros);
                if (!retorno.Exito)
                {
                    return retorno;                    
                }

                string[] partes = retorno.TextoDesarrollador.Split(new string[] { "--{{texto_separacion}}--" }, StringSplitOptions.None);
                string texto_comentarios = partes.Length > 0 ? partes[0] : string.Empty;
                string texto_parametros = partes.Length > 1 ? partes[1] : string.Empty;

                string texto_metodo = "\t\t/// <summary>" + "\n" +
                    "\t\t///" + metodo.Comentario + "\n" +
                    "\t\t/// </summary>\n" +
                    texto_comentarios +
                    "\t\t" + ObtenerTipoVisibilidad(metodo.Visibilidad) +
                    " " + ObtenerTipoDato(metodo.TipoRetorno) +
                    " " + metodo.Nombre + "(" + texto_parametros + ")\n" +
                    "\t\t{\n\n" +
                    "\t\t}\n\n";

                switch (metodo.Visibilidad)
                {
                    case EnumTipoVisibilidad.PUBLICO:
                        texto_metodos_publicos += texto_metodo;
                        break;
                    case EnumTipoVisibilidad.PRIVADO:
                        texto_metodos_privades += texto_metodo;
                        break;
                    case EnumTipoVisibilidad.PROTEGIDO:
                        texto_metodos_protegidos += texto_metodo;
                        break;
                    case EnumTipoVisibilidad.INTERNA:
                        texto_metodos_internos += texto_metodo;
                        break;
                    default:
                        texto_constructores += texto_metodo;
                        break;
                }
            }

            string texto_metodos = string.Empty;
            if (!string.IsNullOrEmpty(texto_constructores))
                texto_metodos = "\t\t#region \"Constructor\"\n\n" + texto_constructores + "\t\t#endregion";
            if (!string.IsNullOrEmpty(texto_metodos_publicos))
                texto_metodos += "\n\n\t\t#region \"Métodos públicos\"\n\n" + texto_metodos_publicos + "\t\t#endregion";
            if (!string.IsNullOrEmpty(texto_metodos_protegidos))
                texto_metodos += "\n\n\t\t#region \"Métodos protegidos\"\n\n" + texto_metodos_protegidos + "\t\t#endregion";
            if (!string.IsNullOrEmpty(texto_metodos_internos))
                texto_metodos += "\n\n\t\t#region \"Métodos internos\"\n\n" + texto_metodos_internos + "\t\t#endregion";
            if (!string.IsNullOrEmpty(texto_metodos_privades))
                texto_metodos += "\n\n\t\t#region \"Métodos privados\"\n\n" + texto_metodos_privades + "\t\t#endregion";

            return new RetornoBase(true,
                "Código generado correctamente.",
                texto_metodos);
        }

        /// <summary>
        /// Genera la definición de los parámetros de un método con sus comentarios.
        /// </summary>
        /// <remarks>
        /// En el retorno se devuelve la definición de los parámetros y los comentarios sobre estos. Para separar el texto se usa este literal "--{{texto_separacion}}--"
        /// </remarks>
        /// <param name="param_parametros">Listado de objetos con la información de los parámetros.</param>
        /// <returns>Objeto con la definición de los parámetros e información del proceso de generación.</returns>
        static private RetornoBase GenerarCodigoParametro(List<Parametro> param_parametros)
        {
            if (param_parametros.Count <= 0)
                return new RetornoBase(true, "Código generado correctamente.", "");

            string comentarios = string.Empty;
            string parametros = string.Empty;
            
            foreach (Parametro parametro in param_parametros)
            {
                // <param name="param_parametros"></param>
                comentarios += "\t\t/// <param name=\"" + parametro.Nombre + "\">" + parametro.Comentario + "</param>\n";
                if (parametros != string.Empty)
                    parametros += ", ";

                parametros += ObtenerTipoDato(parametro.TipoDato) + " " + parametro.Nombre;
            }

            return new RetornoBase(true, 
                "Código generado correctamente.", 
                comentarios + "--{{texto_separacion}}--" + parametros);
        }

        /// <summary>
        /// Genera la definición de los literales de una enumeración con sus comentarios.
        /// </summary>
        /// <param name="param_literales">Listado de objetos de tipo literal cuyo código hay que generar.</param>
        /// <returns>Objeto con la definición de los literales e información del proceso de generación.</returns>
        static private RetornoBase GenerarCodigoLiterales(List<ElementoBase> param_literales)
        {
            if (param_literales.Count <= 0)
                return new RetornoBase(true, "Código generado correctamente.", "");

            string texto_literales = string.Empty;
            foreach(ElementoBase literal in param_literales)
            {
                texto_literales += "\t\t" + literal.Nombre + ",\t\t\t/// <summary>" + literal.Comentario + "</summary>\n";
            }

            return new RetornoBase(true, "Código generado correctamente.", texto_literales);
        }

        /// <summary>
        /// Genera el namespace asociado a una clase según el directorio en el que está guardada la clase.
        /// </summary>
        /// <param name="param_proyecto">Datos del proyecto completo para hacer la búsqueda de la clase según los directorios y subdirectorios.</param>
        /// <param name="param_clase">Clase cuyo namespace se va a generar.</param>
        /// <returns>Objeto con la definición del namespace e información del proceso de generación.</returns>
        static private RetornoBase GenerarNamespace(ProyectoBase param_proyecto, Clase param_clase)
        {
            string texto_namespace = param_proyecto.Nombre;

            foreach (Directorio directorio in param_proyecto.Directorios)
            {
                Clase clase = directorio.ObtenerClasePorId(param_clase.Id);
                if (clase != null)
                    return new RetornoBase(true, "", texto_namespace);

                RetornoBase retorno = GenerarNamespace(directorio, param_clase.Id);
                if (retorno.Exito)
                    return new RetornoBase(true, "", texto_namespace + "." + directorio.Nombre + "." + retorno.TextoDesarrollador);
            }

            return new RetornoBase(false, 
                "No se ha podido generar el texto correctamente.", 
                "No se ha podido generar el namespace correctamente. GenerarNamespace de la clase C_Sharp.");
        }

        /// <summary>
        /// Método recursivo para la búsqueda de la clase según las subcarpetas. Recorre las subcarpetas generando una parte del namespace.
        /// </summary>
        /// <param name="param_directorio">Directorio o subdirectorio que se está recorriendo.</param>
        /// <param name="param_id">Identificador de la clase cuyo namespace se quiere generar.</param>
        /// <returns>Objeto con la definición del namespace e información del proceso de generación.</returns>
        static private RetornoBase GenerarNamespace(Directorio param_directorio, string param_id)
        {
            foreach (Directorio subdirectorio in param_directorio.Subdirectorios)
            {
                Clase clase = subdirectorio.ObtenerClasePorId(param_id);
                if (clase != null)
                    return new RetornoBase(true, "", subdirectorio.Nombre);

                RetornoBase retorno = GenerarNamespace(subdirectorio, param_id);
                if (retorno.Exito)
                    return new RetornoBase(true, "", subdirectorio.Nombre + "." + retorno.TextoDesarrollador);
            }

            return new RetornoBase(false, "No se ha podido generar el texto correctamente.", "No se ha podido generar el namespace correctamente. GenerarNamespace de la clase C_Sharp método recursivo.");
        }

        /// <summary>
        /// Genera el código para identificar una herencia y la implementación de una o más interfaces.
        /// </summary>
        /// <remarks>
        /// En el retorno se devuelve el código y las librerías a importar separadas por el literal "--{{texto_separacion}}--".
        /// La primera clase que se implementa es la heredada, el resto las interfaces. 
        /// Si hay más de una clase heredada el método devuelve error.
        /// </remarks>
        /// <param name="param_proyecto">Objeto con toda la información del proyecto a procesar, para generar los namespace a importar en las clases.</param>
        /// <param name="param_clase_id">Identificador de la clase cuyo código se está generando.</param>
        /// <param name="param_asociaciones">Listado de todas las asociaciones de la clase cuyo código se está generando.</param>
        /// <returns>Objeto con la definición del namespace e información del proceso de generación.</returns>
        static private RetornoBase GenerarHerencia(ProyectoBase param_proyecto, string param_clase_id, List<Asociacion> param_asociaciones)
        {
            List<Asociacion> herencias = param_asociaciones.FindAll(a => a.TipoAsociacion == EnumTipoAsociacion.HERENCIA);

            bool clase_herenciada = false;
            string texto_herencia = string.Empty;
            string texto_interfaces = string.Empty;
            string librerias = string.Empty;
            foreach (Asociacion herencia in herencias)
            {
                Clase clase_origen = BuscarComponentes.BuscarClase(param_proyecto, herencia.IdClaseOrigen);
                Clase clase_destino = BuscarComponentes.BuscarClase(param_proyecto, herencia.IdClaseDestino);

                if (clase_origen == null || clase_destino == null)
                    return new RetornoBase(false, 
                        "No se ha podido generar el código correctamente.", 
                        "Clase C_Sharp, método generar herencia, asociación con id = " + herencia.Id + " la clase origen o la clase destino son null");

                if (clase_destino.Id != param_clase_id)
                    continue;

                if (clase_origen.TipoClase != EnumTiposClases.INTERFAZ)
                {
                    if (clase_herenciada)
                        return new RetornoBase(false, 
                            "No se ha podido generar el código correctamente.", 
                            "Clase C_Sharp, método generar herencia, una misma clase hereda de dos clases (no interfaces) simultaneamente.");
                    else
                        clase_herenciada = true;
                }

                if (texto_interfaces != string.Empty)
                    texto_interfaces += ", ";

                if (clase_origen.TipoClase == EnumTiposClases.INTERFAZ)
                    texto_interfaces += clase_origen.Nombre;
                else
                    texto_herencia += clase_origen.Nombre;

                RetornoBase retorno = GenerarNamespace(param_proyecto, clase_origen);
                if (!retorno.Exito)
                    return retorno;

                librerias += "using " + retorno.TextoDesarrollador + ";\n";
            }

            string texto_retorno = texto_interfaces == string.Empty ? texto_herencia : texto_herencia + ", " + texto_interfaces;
            return new RetornoBase(true,
                "",
                librerias + "--{{texto_separacion}}--" + texto_retorno);
        }

        /// <summary>
        /// Genera las librerías internas que necesita la clase cuyo código se está generando para funcionar correctamente según sus asociaciones.
        /// </summary>
        /// <remarks>
        /// El método hace un listado de todas las asociaciones a excepción de las herencias que se gestionan en otro método.
        /// El método no permite duplicidades de importaciones.
        /// </remarks>
        /// <param name="param_proyecto">Objeto con toda la información del proyecto a procesar, para generar los namespace.</param>
        /// <param name="param_clase_id">Identificador de la clase que se está procesando.</param>
        /// <param name="param_asociaciones">Listado de todas las asociaciones definidas en la clase que se está procesando.</param>
        /// <returns></returns>
        static private RetornoBase GenerarAsociaciones(ProyectoBase param_proyecto, string param_clase_id, List<Asociacion> param_asociaciones)
        {
            List<Asociacion> asociaciones = param_asociaciones.FindAll(a => (a.TipoAsociacion != EnumTipoAsociacion.NO_DEFINIDA && a.TipoAsociacion != EnumTipoAsociacion.HERENCIA));

            string texto_librearias = string.Empty;
            foreach (Asociacion asociacion in asociaciones)
            {
                if (asociacion.IdClaseOrigen != param_clase_id)
                    continue;

                Clase clase_origen = BuscarComponentes.BuscarClase(param_proyecto, asociacion.IdClaseOrigen);
                Clase clase_destino = BuscarComponentes.BuscarClase(param_proyecto, asociacion.IdClaseDestino);

                if (clase_origen == null || clase_destino == null)
                    return new RetornoBase(false,
                        "No se ha podido generar el código correctamente.",
                        "Clase C_Sharp, método generar asociaciones, asociación con id = " + asociacion.Id + " la clase origen o la clase destino son null");

                RetornoBase retorno = GenerarNamespace(param_proyecto, clase_origen);
                if (!retorno.Exito)
                    return retorno;

                if (!texto_librearias.Contains(retorno.TextoDesarrollador))
                    texto_librearias += "using " + retorno.TextoDesarrollador + ";\n";
            }

            return new RetornoBase(true, "", texto_librearias);
        }

        #endregion

        #region "Transformacion de enumeraciones"

        /// <summary>
        /// Genera el literal en C# según la enumeración pasada por parámetro.
        /// </summary>
        /// <param name="param_tipo_clase">Enumeración de tipo de clase.</param>
        /// <returns>Literal en código C#</returns>
        static public string ObtenerTipoClase(EnumTiposClases param_tipo_clase)
        {
            switch (param_tipo_clase)
            {
                case EnumTiposClases.CLASE_ABSTRACTA:
                    return "abstract class";
                case EnumTiposClases.CLASE:
                    return "class";
                case EnumTiposClases.INTERFAZ:
                    return "interface";
                case EnumTiposClases.ENUMERACION:
                    return "enum";
                default:
                    return "class";
            }
        }

        /// <summary>
        /// Genera el literal en C# según la enumeración pasada por parámetro.
        /// </summary>
        /// <param name="param_visibilidad">Enumeración de tipo de visibilidad.</param>
        /// <returns>Literal en código C#</returns>
        static public string ObtenerTipoVisibilidad(EnumTipoVisibilidad param_visibilidad)
        {
            switch (param_visibilidad)
            {
                case EnumTipoVisibilidad.PUBLICO:
                    return "public";
                case EnumTipoVisibilidad.PRIVADO:
                    return "private";
                case EnumTipoVisibilidad.PROTEGIDO:
                    return "protected";
                case EnumTipoVisibilidad.INTERNA:
                    return "internal"; 
                default:
                    return "public";
            }
        }

        /// <summary>
        /// Genera el literal en C# según la enumeración pasada por parámetro.
        /// </summary>
        /// <param name="param_tipo_dato">Enumeración de tipo de dato.</param>
        /// <returns>Literal en código C#</returns>
        static public string ObtenerTipoDato(EnumTipoDato param_tipo_dato)
        {
            switch (param_tipo_dato)
            {
                case EnumTipoDato.ENTERO:
                    return "int";
                case EnumTipoDato.FLOTANTE:
                    return "float";
                case EnumTipoDato.LARGO:
                    return "long";
                case EnumTipoDato.CORTO:
                    return "short";
                case EnumTipoDato.DOBLE:
                    return "double";
                case EnumTipoDato.BOOLEANO:
                    return "bool";
                case EnumTipoDato.CARACTER:
                    return "char";
                case EnumTipoDato.CADENA:
                    return "string";
                case EnumTipoDato.BYTE:
                    return "byte";
                default:
                    return "void";
            }
        }
        
        #endregion
    }
}
