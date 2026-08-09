using System;
using System.Windows.Forms;

namespace transformador_mda.src.Features.Utilidades
{
    /// <summary>
    /// Clase para gestionar diferentes operaciones relacionadas con carpetas.
    /// </summary>
    internal class Folder
    {
        /// <summary>
        /// Abre el explorador de archivos para seleccionar un directorio, con un título personalizado.
        /// </summary>
        /// <param name="param_description">Descripción que se mostrará en el explorador de carpetas.</param>
        /// <returns>Ruta del directorio seleccionado, vacío si el usuario cancela.</returns>
        static public string GetFolderPath(string param_description)
        {
            var dialogo = new FolderBrowserDialog();
            dialogo.Description = param_description;
            dialogo.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialogo.ShowNewFolderButton = true;
            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                return dialogo.SelectedPath;
            }

            return string.Empty;
        }
    }
}
