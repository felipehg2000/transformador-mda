using System;
using System.IO;
using System.Text.Json;
using transformador_mda.src.Dominio;

namespace transformador_mda.src.Features.GestionConfiguracion
{
    /// <summary>
    /// Clase para controlar la serialización y deserialización a JSON.
    /// </summary>
    public class DAO
    {
        /// <summary>
        /// Importación de JSON a objeto Configuración.
        /// </summary>
        /// <param name="param_fichero">Ruta del fichero donde se almacena la información en JSON.</param>
        /// <returns>Objeto de tipo Configuración con los datos almacenados en el fichero.</returns>
        /// <exception cref="FileNotFoundException">Excepción de fichero no encontrado.</exception>
        /// <exception cref="JsonException">Excepción de deserialización.</exception>
        static public Configuracion Importar(string param_fichero)
        {
            if (!File.Exists(param_fichero))
                throw new FileNotFoundException("El archivo de configuración no existe.", param_fichero);

            string jsonConfiguracion = File.ReadAllText(param_fichero);

            Configuracion retConfiguracion = JsonSerializer.Deserialize<Configuracion>(jsonConfiguracion);
            if (retConfiguracion == null)
                throw new JsonException("Error al desearializar el archivo de configuración.");

            return retConfiguracion;
        }

        /// <summary>
        /// Exportación del objeto Configuración a un formato JSON.
        /// </summary>
        /// <param name="param_configuracion">Objeto de tipo Configuración.</param>
        /// <param name="param_directorio">Ruta al directorio donde se almacenará la información en formato JSON.</param>
        /// <param name="param_nombre_fichero">Nombre del fichero a generar y en el que almacenar la información. En caso de estar a vacío se genera un nombre automáticamente.</param>
        static public void Exportar(Configuracion param_configuracion, string param_directorio, string param_nombre_fichero = null)
        {
            string nombreFichero = string.Empty;
            if (string.IsNullOrEmpty(nombreFichero))
            {
                nombreFichero = DateTime.Now.ToString("yyyyMMddHHmmss") + ".json";
            }
            else
            {
                if (Path.GetExtension(param_nombre_fichero) != ".json")
                    nombreFichero = param_nombre_fichero + ".json";
                else
                    nombreFichero = param_nombre_fichero;
            }

            param_directorio = Path.Combine(param_directorio, nombreFichero);

            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(param_configuracion, opciones);

            File.WriteAllText(param_directorio, jsonString);
        }
    }
}
