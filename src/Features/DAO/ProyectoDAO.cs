using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;

namespace transformador_mda.src.Features.DAO
{
    /// <summary>
    /// Genera un fichero JSON de la información del proyecto UML que se ha leido por el diagrama de clases.
    /// </summary>
    /// <remarks>
    /// Se utiliza para comunicar a la IA toda la información del proyecto al generar el código por IA.
    /// </remarks>
    public class ProyectoDAO
    {
        /// <summary>
        /// Serializa un fichero de tipo JSON con los datos de un proyecto.
        /// </summary>
        /// <remarks>
        /// En vez de llevar las claves de las enumeraciones se lleva su literal para que la IA comprenda a qué se refiere cada clave de las enumeraciones, sin explicaciones a mayores.
        /// </remarks>
        /// <param name="param_proyecto">Información a serializar en un proyecto JSON.</param>
        /// <param name="param_directorio">Directorio donde se almacenará el fichero JSON generado.</param>
        /// <param name="param_nombre_fichero">Nombre del fichero que se generará, por defecto NULL para que se genere el nombre automáticamente.</param>
        /// <returns></returns>
        public static RetornoBase Exportar(ProyectoBase param_proyecto, string param_directorio, string param_nombre_fichero = null)
        {
            if (param_proyecto == null)
            {
                return new RetornoBase(false, "No se puede exportar un proyecto nulo.", string.Empty);
            }

            if (string.IsNullOrWhiteSpace(param_directorio))
            {
                return new RetornoBase(false, "El directorio de exportación no es válido.", string.Empty);
            }

            string nombre_fichero;

            if (string.IsNullOrWhiteSpace(param_nombre_fichero))
            {
                nombre_fichero = DateTime.Now.ToString("yyyyMMddHHmmss") + ".json";
            }
            else
            {
                if (!string.Equals(Path.GetExtension(param_nombre_fichero), ".json", StringComparison.OrdinalIgnoreCase))
                {
                    nombre_fichero = param_nombre_fichero + ".json";
                }
                else
                {
                    nombre_fichero = param_nombre_fichero;
                }
            }

            string ruta_fichero = Path.Combine(param_directorio, nombre_fichero);

            try
            {
                JsonSerializerOptions opciones = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                opciones.Converters.Add(new JsonStringEnumConverter());

                string json_string = JsonSerializer.Serialize(param_proyecto, opciones);

                File.WriteAllText(ruta_fichero, json_string);
                return new RetornoBase(true, string.Empty, ruta_fichero);
            }
            catch (Exception e)
            {
                return new RetornoBase(false, "Error al exportar el proyecto a JSON.", e.Message);
            }
        }
    }
}