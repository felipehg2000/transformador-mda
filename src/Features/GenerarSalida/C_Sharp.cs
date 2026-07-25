using System;
using System.Collections.Generic;
using transformador_mda.src.Dominio.Retornos;
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

        public override RetornoBase GenerarSalida()
        {
            throw new NotImplementedException();
        }

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

        static private RetornoBase GenerarCodigoClases(ProyectoBase param_proyecto, Clase param_clase)
        {
            string texto_clases = "namespace " + GenerarNamespace(param_proyecto, param_clase).TextoDesarrollador + "\n" +
                "{\n" +
                "\t" + ObtenerTipoVisibilidad(param_clase.Visibilidad) + " " + ObtenerTipoClase(param_clase.TipoClase) + " " + param_clase.Nombre + "\n" +
                "\t{\n";


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

        static private RetornoBase GenerarCodigoLiterales(List<ElementoBase> param_literales)
        {
            if (param_literales.Count <= 0)
                return new RetornoBase(true, "Código generado correctamente.", "");

            string texto_literales = string.Empty;
            foreach(ElementoBase literal in param_literales)
            {
                texto_literales += "\t\t" + literal.Nombre + ", <summary>" + literal.Comentario + "</summary>\n";
            }

            return new RetornoBase(true, "Código generado correctamente.", texto_literales);
        }

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

            return new RetornoBase(false, "No se ha podido generar el texto correctamente.", "No se ha podido generar el namespace correctamente. GenerarNamespace de la clase C_Sharp.");
        }

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

        #endregion

        #region "Transformacion de enumeraciones"

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
