using System;
using System.Windows.Forms;

namespace transformador_mda.src.Features.ExploradorFicheros
{
    /// <summary>
    /// Clase para gestionar diferentes operaciones relacionadas con archivos.
    /// </summary>
    internal class Files
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

        #endregion "File reading and writing"
    }
}
