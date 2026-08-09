using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Dominio.ElementosUML;

namespace transformador_mda.src.Features.DAO
{
    public class ProyectoDAO
    {
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