using System;
using System.IO;
using System.Windows.Forms;
using transformador_mda.src.Dominio.Retornos;

namespace transformador_mda.src.Features.Utilidades
{
    /// <summary>
    /// Clase para gestionar diferentes operaciones relacionadas con archivos.
    /// </summary>
    internal class Ficheros
    {
        #region "File explorer"

        /// <summary>
        /// Abre el explorador de archivos para seleccionar un archivo, con un título y filtro personalizados.
        /// </summary>
        /// <param name="param_title">Nombre del archivo a buscar que se mostrará en el explorador de archivos.</param>
        /// <param name="param_filter">Filtro de extensión de archivos.</param>
        /// <returns>Vacío si el usuario ha cancelado el proceso, ruta del archivo seleccionado si no.</returns>
        static public string GetFilePath(string param_title, string param_filter)
        {
            var dialogo = new OpenFileDialog();
            dialogo.Title = param_title;
            dialogo.Filter = param_filter;
            dialogo.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialogo.Multiselect = false;

            if (dialogo.ShowDialog() != DialogResult.OK)
            {
                return string.Empty;
            }

            return dialogo.FileName;
        }

        #endregion "File explorer"

        #region "File reading and writing"

        /// <summary>
        /// Comprueba si una ruta existe, si existe lee el contenido y lo retorna.
        /// </summary>
        /// <param name="param_path">Ruta al archivo que se quiere leer.</param>
        /// <returns>Contenido del archivo si existe, vacío si no.</returns>
        /// <exception cref="Exception">En la ruta pasada por parámetro no existe ningún archivo.</exception>
        static public string[] ReadFileLines(string param_path)
        {
            if (!System.IO.File.Exists(param_path))
            {
                throw new Exception("No se ha encontrado la ruta especificada.");
            }

            string[] ret_lineas = System.IO.File.ReadAllLines(param_path);
            if (ret_lineas == null || ret_lineas.Length == 0)
            {
                return new string[0];
            }

            return ret_lineas;
        }

        /// <summary>
        /// Escribe la información pasada por parámetro en un archivo, en la ruta pasada por parámetros.
        /// </summary>
        /// <param name="param_path">Ruta donde se almacenará el fichero.</param>
        /// <param name="param_lines">Información que tendrá el fichero.</param>
        static public void WriteFileLines(string param_path, string[] param_lines)
        {
            System.IO.File.WriteAllLines(param_path, param_lines);
        }

        /// <summary>
        /// Escribe el texto pasado por parámetro en un archivo, en la ruta pasada por parámetros.
        /// </summary>
        /// <param name="param_path">Ruta donde se almacenará el fichero.</param>
        /// <param name="param_text">Texto que tendrá el fichero.</param>
        static public void WriteFileText(string param_path, string param_text)
        {
            System.IO.File.WriteAllText(param_path, param_text);
        }

        /// <summary>
        /// Escribe un fichero temporal con el contenido pasado por parámetro, en la ruta pasada por parámetros, con la extensión pasada por parámetro. Genera el nombre de manera automática.
        /// </summary>
        /// <param name="param_ruta">Directorio donde se almacenará el fichero.</param>
        /// <param name="param_extension">Extensión del fichero.</param>
        /// <param name="param_contenido">Contenido del fichero.</param>
        /// <returns>Objeto RetornoBase con el resultado de la operación o información del proceso.</returns>
        static public RetornoBase EscribirFichero(string param_ruta, string param_extension, string param_contenido)
        {
            if (!Directory.Exists(param_ruta))
            {
                Directory.CreateDirectory(param_ruta);
            }

            try
            {
                string nombre_fichero = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + param_extension;
                string ruta_completa = Path.Combine(param_ruta, nombre_fichero);
                File.WriteAllText(ruta_completa, param_contenido);
                return new RetornoBase(true, string.Empty, ruta_completa);
            }
            catch (Exception e)
            {
                return new RetornoBase(false, "No se ha podido crear el fichero temporal con el código esqueleto.", e.Message);
            }
        }

        /// <summary>
        /// Borra un fichero en la ruta pasada por parámetro, si existe.
        /// </summary>
        /// <param name="param_ruta">Ruta del fichero a borrar.</param>
        /// <returns>True si el fichero fue borrado, false en caso contrario.</returns>
        static public bool BorrarFichero(string param_ruta)
        {
            try
            {
                if (File.Exists(param_ruta))
                {
                    File.Delete(param_ruta);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion "File reading and writing"
    }
}
