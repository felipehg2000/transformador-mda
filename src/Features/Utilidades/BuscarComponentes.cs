using transformador_mda.src.Dominio.ElementosUML;

namespace transformador_mda.src.Features.Utilidades
{
    /// <summary>
    /// Utilidad para buscar componentes según cierta información.
    /// </summary>
    static class BuscarComponentes
    {
        #region "Buscar por ID"

        /// <summary>
        /// Método para buscar una asociación por su id.
        /// </summary>
        /// <param name="param_proyecto">Información del proyecto en gestión.</param>
        /// <param name="param_id">Identificador de la asociación a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static public Asociacion BuscarAsociacion(ProyectoBase param_proyecto, string param_id)
        {
            foreach (Directorio directorio in param_proyecto.Directorios)
            {
                Asociacion asociacion = BuscarAsociacion(directorio, param_id);
                if (asociacion != null)
                    return asociacion;
            }

            return null;
        }

        /// <summary>
        /// Método recursivo para buscar una asociación por su id en las diferentes subcarpetas.
        /// </summary>
        /// <param name="param_directorio">Directorio en el que buscará la asociación y cuyos subdirectorios recorrerá.</param>
        /// <param name="param_id">Identificador de la asociación a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static private Asociacion BuscarAsociacion(Directorio param_directorio, string param_id)
        {
            foreach (Clase clase in param_directorio.Clases)
            {
                Asociacion asociacion = clase.ObtenerAsociacionPorId(param_id);
                if (asociacion != null)
                    return asociacion;
            }

            foreach (Directorio subdirectorio in param_directorio.Subdirectorios)
            {
                Asociacion asociacion = BuscarAsociacion(subdirectorio, param_id);
                if (asociacion != null)
                    return asociacion;
            }

            return null;
        }

        /// <summary>
        /// Método para buscar un atributo por su id.
        /// </summary>
        /// <param name="param_proyecto">Información del proyecto en gestión.</param>
        /// <param name="param_id">Identificador del atributo a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static public Atributo BuscarAtributo(ProyectoBase param_proyecto, string param_id)
        {
            foreach (Directorio directorio in param_proyecto.Directorios)
            {
                Atributo atributo = BuscarAtributo(directorio, param_id);
                if (atributo != null)
                    return atributo;
            }

            return null;
        }

        /// <summary>
        /// Método recursivo para buscar un atributo por su id en las diferentes subcarpetas.
        /// </summary>
        /// <param name="param_directorio">Directorio en el que buscará el atributo y cuyos subdirectorios recorrerá.</param>
        /// <param name="param_id">Identificador del atributo a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static private Atributo BuscarAtributo(Directorio param_directorio, string param_id)
        {
            foreach (Clase clase in param_directorio.Clases)
            {
                Atributo atributo = clase.ObtenerAtributoPorId(param_id);
                if (atributo != null)
                    return atributo;
            }

            foreach (Directorio subdirectorio in param_directorio.Subdirectorios)
            {
                Atributo atributo = BuscarAtributo(subdirectorio, param_id);
                if (atributo != null)
                    return atributo;
            }

            return null;
        }

        /// <summary>
        /// Método para buscar una clase por su id.
        /// </summary>
        /// <param name="param_proyecto">Información del proyecto en gestión.</param>
        /// <param name="param_id">Identificador de la clase a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static public Clase BuscarClase(ProyectoBase param_proyecto, string param_id)
        {
            foreach (Directorio directorio in param_proyecto.Directorios)
            {
                Clase clase = BuscarClase(directorio, param_id);
                if (clase != null) 
                    return clase;
            }

            return null;
        }

        /// <summary>
        /// Método recursivo para buscar una clase por su id en las diferentes subcarpetas.
        /// </summary>
        /// <param name="param_directorio">Directorio en el que buscará la clase y cuyos subdirectorios recorrerá.</param>
        /// <param name="param_id">Identificador de la clase a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static private Clase BuscarClase(Directorio param_directorio, string param_id)
        {
            Clase clase = param_directorio.ObtenerClasePorId(param_id);
            if (clase != null)
                return clase;

            foreach (Directorio subdirectorio in param_directorio.Subdirectorios)
            {
                clase = BuscarClase(subdirectorio, param_id);
                if (clase != null)
                    return clase;
            }

            return null;
        }

        /// <summary>
        /// Método para buscar un método por su id.
        /// </summary>
        /// <param name="param_proyecto">Información del proyecto en gestión.</param>
        /// <param name="param_id">Identificador del método a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static public Metodo BuscarMetodo(ProyectoBase param_proyecto, string param_id)
        { 
            foreach (Directorio directorio in param_proyecto.Directorios)
            {
                Metodo metodo = BuscarMetodo(directorio, param_id);
                if (metodo != null)
                    return metodo;
            }

            return null;
        }

        /// <summary>
        /// Método recursivo para buscar un método por su id en las diferentes subcarpetas.
        /// </summary>
        /// <param name="param_directorio">Directorio en el que buscará el método y cuyos subdirectorios recorrerá.</param>
        /// <param name="param_id">Identificador del método a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static private Metodo BuscarMetodo(Directorio param_directorio, string param_id)
        {
            foreach (Clase clase in param_directorio.Clases)
            {
                Metodo metodo = clase.ObtenerMetodoPorId(param_id);
                if (metodo != null)
                    return metodo;
            }

            foreach(Directorio subdirectorio in param_directorio.Subdirectorios)
            {
                Metodo metodo = BuscarMetodo(subdirectorio, param_id);
                if (metodo != null)
                    return metodo;
            }

            return null;
        }

        /// <summary>
        /// Método para buscar un parámetro por su id.
        /// </summary>
        /// <param name="param_proyecto">Información del proyecto en gestión.</param>
        /// <param name="param_id">Identificador del parámetro a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static public Parametro BuscarParametro(ProyectoBase param_proyecto, string param_id)
        {
            foreach (Directorio directorio in param_proyecto.Directorios)
            {
                Parametro parametro = BuscarParametro(directorio, param_id);
                if (parametro != null)
                    return parametro;
            }

            return null;
        }

        /// <summary>
        /// Método recursivo para buscar un parámetro por su id en las diferentes subcarpetas.
        /// </summary>
        /// <param name="param_directorio">Directorio en el que buscará el parámetro y cuyos subdirectorios recorrerá.</param>
        /// <param name="param_id">Identificador del parámetro a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static private Parametro BuscarParametro(Directorio param_directorio, string param_id)
        {
            foreach (Clase clase in param_directorio.Clases)
            {
                foreach (Metodo metodo in clase.Metodos)
                {
                    Parametro parametro = metodo.BuscarParametroPorId(param_id);
                    if (parametro != null)
                        return parametro;
                }
            }

            foreach (Directorio subdirectorio in param_directorio.Subdirectorios)
            {
                Parametro parametro = BuscarParametro(subdirectorio, param_id);
                if (parametro != null)
                    return parametro;
            }

            return null;
        }

        /// <summary>
        /// Método para buscar un literal por su id.
        /// </summary>
        /// <param name="param_proyecto">Información del proyecto en gestión.</param>
        /// <param name="param_id">Identificador del literal a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static public ElementoBase BuscarLiteral(ProyectoBase param_proyecto, string param_id)
        {
            foreach (Directorio directorio in param_proyecto.Directorios)
            {
                ElementoBase literal = BuscarLiteral(directorio, param_id);
                if (literal != null)
                    return literal;
            }

            return null;
        }

        /// <summary>
        /// Método recursivo para buscar un literal por su id en las diferentes subcarpetas.
        /// </summary>
        /// <param name="param_directorio">Directorio en el que buscará el literal y cuyos subdirectorios recorrerá.</param>
        /// <param name="param_id">Identificador del literal a buscar.</param>
        /// <returns>Elemento UML con identificador igual al pasado por parámetros.</returns>
        static private ElementoBase BuscarLiteral(Directorio param_directorio, string param_id)
        {
            foreach (Clase clase in param_directorio.Clases)
            {
                ElementoBase literal = clase.ObtenerLiteralPorId(param_id);
                if (literal != null)
                    return literal;
            }

            foreach (Directorio subdirectorio in param_directorio.Subdirectorios)
            {
                ElementoBase literal = BuscarLiteral(subdirectorio, param_id);
                if (literal != null)
                    return literal;
            }

            return null;
        }

        #endregion
    }
}
