using OpenAI;
using OpenAI.Chat;
using OpenAI.Files;
using OpenAI.Responses;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;

namespace transformador_mda.src.Features.IntegracionIA
{
    public class OpenAI : IProveedorIA
    {
        /// <summary>
        /// Clave de la API de OpenAI.
        /// </summary>
        private const string Token = "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";

        private const string Prompt =
@"Actúa como un desarrollador sénior especializado en C# y .NET.

Tu única tarea es implementar los cuerpos de los métodos indicados en el código esqueleto proporcionado, respetando estrictamente la arquitectura, las clases, las relaciones, los atributos, las firmas y la documentación definidas por el usuario.

VERSIÓN OBJETIVO DE .NET:
[[VERSION_DOTNET]]

NOMBRE DE LA CLASE:
[[NOMBRE_CLASE]]

==================================================
REGLAS OBLIGATORIAS
==================================================

1. No modifiques, elimines ni renombres:
   - Clases.
   - Interfaces.
   - Enumeraciones.
   - Namespaces.
   - Atributos.
   - Propiedades.
   - Constructores.
   - Métodos.
   - Parámetros.
   - Tipos de retorno.
   - Modificadores de acceso.
   - Modificadores static, abstract, virtual, override, async o similares.
   - Relaciones de herencia o implementación.

2. No añadas nuevos métodos públicos, protegidos o internos.

3. Implementa exclusivamente los métodos que contienen indicadores con el formato:

   {{IDENTIFICADOR}}

4. Cada identificador encontrado en el código esqueleto debe aparecer exactamente una vez en el array ""codigo"" de la respuesta.

5. No inventes identificadores.

6. El valor de ""codigo"" correspondiente a cada identificador debe contener únicamente el código que sustituirá al indicador.

7. No incluyas en ese valor:
   - La firma del método.
   - El nombre del método.
   - Las llaves exteriores del método.
   - Bloques Markdown.
   - Delimitadores de código.
   - Explicaciones externas.
   - El indicador {{IDENTIFICADOR}}.

8. El código generado debe:
   - Ser C# válido.
   - Ser compatible con la versión de .NET indicada.
   - Compilar dentro del esqueleto proporcionado.
   - Respetar el carácter síncrono o asíncrono de cada método.
   - Utilizar correctamente await cuando el método sea async.
   - Devolver un valor compatible con el tipo de retorno.
   - No utilizar variables, propiedades, métodos o clases inexistentes.
   - Utilizar los atributos y dependencias disponibles en la clase.
   - Respetar la responsabilidad descrita en los comentarios.
   - Evitar código duplicado.
   - Evitar efectos secundarios no solicitados.
   - Realizar validaciones razonables cuando sean necesarias.
   - No utilizar bloques catch vacíos.
   - No introducir credenciales, claves API, rutas locales ni datos sensibles.

9. Usa como fuente de verdad, por este orden:
   a. La firma del método.
   b. La documentación del método.
   c. La información estructurada del proyecto.
   d. Los atributos y métodos disponibles en la clase.
   e. El código esqueleto.

10. No contradigas la documentación proporcionada.

11. No cambies el diseño para facilitar la implementación.

12. No añadas funcionalidades no solicitadas.

13. Si un comportamiento no está completamente especificado:
   - Realiza la implementación mínima compatible con la documentación.
   - No inventes reglas de negocio importantes.
   - No añadas conexiones externas, acceso a bases de datos, escritura de archivos o llamadas de red no indicadas.
   - Indica brevemente la limitación en ""comentarioPrincipal"".

14. Utiliza preferentemente las bibliotecas incluidas en .NET.

15. Solo recomienda un paquete NuGet cuando la funcionalidad no pueda implementarse razonablemente mediante:
   - La biblioteca estándar de .NET.
   - Las dependencias ya declaradas.
   - Una implementación sencilla y mantenible.

16. Evita crear funciones auxiliares.

17. Solo crea una función auxiliar cuando sea estrictamente necesaria para evitar una duplicación significativa o separar una operación compleja.

18. Toda función auxiliar generada debe ser:
   - private.
   - Compatible con la versión de .NET indicada.
   - Autocontenida.
   - Utilizada al menos por uno de los métodos generados.
   - Incluida completamente en ""funcionesAuxiliares"".

19. No generes funciones auxiliares sin uso.

20. No incluyas cláusulas using dentro del código de los métodos.

21. Incluye en ""libreriasNecesarias"" únicamente los using adicionales que realmente sean necesarios y que no aparezcan ya en el esqueleto.

22. Cada elemento de ""libreriasNecesarias"" debe contener una cláusula using completa, incluyendo el punto y coma.

23. En ""paquetesNuGet"":
   - Incluye únicamente paquetes imprescindibles.
   - Indica el nombre real del paquete.
   - Indica una versión compatible cuando pueda determinarse con seguridad.
   - Si la versión no puede determinarse con seguridad, utiliza una cadena vacía.
   - Explica brevemente por qué resulta necesario.

24. El campo ""comentarioPrincipal"" debe contener únicamente información relevante para integrar y utilizar correctamente el código generado:
   - Suposiciones realizadas.
   - Configuración necesaria.
   - Limitaciones.
   - Dependencias externas.
   - Paquetes NuGet necesarios.
   - Requisitos para su funcionamiento.

25. Si no existe información relevante para ""comentarioPrincipal"", devuelve una cadena vacía.

==================================================
FORMATO DE RESPUESTA OBLIGATORIO
==================================================

Devuelve exclusivamente un único objeto JSON válido con esta estructura exacta:

{
  ""codigo"": [
    {
      ""id"": ""IDENTIFICADOR_EXACTO"",
      ""codigo"": ""CUERPO_DEL_METODO""
    }
  ],
  ""libreriasNecesarias"": [
    ""using Namespace.Necesario;""
  ],
  ""funcionesAuxiliares"": [
    {
      ""codigo"": ""CODIGO_COMPLETO_DE_LAS_FUNCIONES_AUXILIARES""
    }
  ],
  ""paquetesNuGet"": [
    {
      ""nombre"": ""Nombre.Del.Paquete"",
      ""version"": ""VersionCompatible"",
      ""motivo"": ""Razón por la que resulta imprescindible""
    }
  ],
}

RESTRICCIONES DEL JSON:

- La raíz debe ser un objeto JSON.
- Debe contener siempre las cinco propiedades indicadas.
- ""codigo"" debe ser siempre un array.
- ""libreriasNecesarias"" debe ser siempre un array de cadenas.
- ""funcionesAuxiliares"" debe ser siempre un array.
- ""paquetesNuGet"" debe ser siempre un array.
- ""comentarioPrincipal"" debe ser siempre una cadena.
- Cuando no haya elementos, devuelve arrays vacíos.
- Los saltos de línea contenidos dentro de cadenas JSON deben representarse mediante \n.
- Las comillas contenidas dentro del código C# deben escaparse correctamente para producir JSON válido.
- Las barras invertidas deben escaparse correctamente.
- No añadas propiedades diferentes.
- No escribas texto antes ni después del objeto JSON.
- No utilices Markdown.
- No incluyas comentarios dentro del JSON.
- No devuelvas varias alternativas.
- Verifica que la respuesta pueda procesarse mediante System.Text.Json.JsonDocument.Parse.

==================================================
INFORMACIÓN ESTRUCTURADA DEL PROYECTO
==================================================

Utiliza la siguiente información como contexto estructural del proyecto. Puede contener clases, atributos, métodos, parámetros, relaciones y documentación funcional.

No generes código para clases o métodos distintos de los presentes en el código esqueleto.

[[CONTENIDO_JSON]]

==================================================
CÓDIGO ESQUELETO DE LA CLASE
==================================================

[[CODIGO_ESQUELETO]]

==================================================
COMPROBACIÓN FINAL INTERNA
==================================================

Antes de responder, comprueba internamente que:

1. Has encontrado todos los indicadores con formato {{IDENTIFICADOR}}.
2. Cada indicador aparece exactamente una vez en ""codigo"".
3. No has añadido identificadores inexistentes.
4. Cada fragmento sustituye únicamente al indicador correspondiente.
5. No has alterado firmas ni estructura.
6. Todas las funciones auxiliares son private.
7. Todas las funciones auxiliares son utilizadas.
8. Las librerías indicadas son realmente necesarias.
9. Los paquetes NuGet indicados son imprescindibles.
10. La respuesta completa es JSON válido.
11. No existe ningún texto fuera del JSON.

Devuelve exclusivamente el objeto JSON.";

        public async Task<RetornoBase> GenerarCodigo(Clase param_clase, string param_ruta_json, string param_esqueleto)
        {
            try
            {
                // Crear cliente
                ChatClient cliente = new ChatClient(model: "gpt-5.6-luna", apiKey: Token);
                RetornoBase retorno_subida = await SubirFichero(param_ruta_json);

                if (!retorno_subida.Exito)
                    return retorno_subida;


                // Enviar la petición
                ChatCompletion respuesta = await cliente.CompleteChatAsync(Prompt);

                // Obtener la respuesta
                string resultado = respuesta.Content[0].Text;

                // Devolver la respuesta
                return new RetornoBase(true, "Correcto", resultado);
            }
            catch (ClientResultException e)
            {
                return ControlarExcepcion(e);
            }
        }

        private RetornoBase ControlarExcepcion(ClientResultException param_excepcion)
        {
            // Datos del error
            int status = param_excepcion.Status;
            string detalleTecnico = param_excepcion.Message;
            string mensajeUsuario = "No se ha podido generar el código correctamente.";

            // Intentar obtener el código y tipo interno devuelto por OpenAI
            string codigoInterno = string.Empty;
            string tipoInterno = string.Empty;
            try
            {
                string json = param_excepcion.GetRawResponse().Content.ToString();

                JsonDocument documento = JsonDocument.Parse(json);
                if (documento.RootElement.TryGetProperty("error", out JsonElement error))
                {
                    if (error.TryGetProperty("code", out JsonElement code))
                    {
                        codigoInterno = code.GetString() ?? string.Empty;
                    }

                    if (error.TryGetProperty("type", out JsonElement type))
                    {
                        tipoInterno = type.GetString() ?? string.Empty;
                    }
                }
            }
            catch
            {
                // Si no se puede obtener el JSON, simplemente se continúa.
            }

            if (status == 400)
            {
                mensajeUsuario = "La solicitud enviada a la inteligencia artificial no es válida.";
            }
            else if (status == 401)
            {
                mensajeUsuario = "La clave API introducida no es válida.";
            }
            else if (status == 403)
            {
                mensajeUsuario = "La cuenta no dispone de permisos para utilizar la inteligencia artificial.";
            }
            else if (status == 404)
            {
                mensajeUsuario = "El modelo de inteligencia artificial solicitado no está disponible.";
            }
            else if (status == 408)
            {
                mensajeUsuario = "La operación ha superado el tiempo máximo de espera.";
            }
            else if (status == 429 && (codigoInterno.Equals("insufficient_quota", StringComparison.OrdinalIgnoreCase) ||
                      tipoInterno.Equals("insufficient_quota", StringComparison.OrdinalIgnoreCase)))
            {
                mensajeUsuario = "La cuenta asociada a la clave API no dispone de crédito suficiente.";
            }
            else if (status == 429 && (codigoInterno.Equals("rate_limit_exceeded", StringComparison.OrdinalIgnoreCase) ||
                      tipoInterno.Equals("rate_limit_exceeded", StringComparison.OrdinalIgnoreCase)))
            {
                mensajeUsuario = "Se ha superado temporalmente el límite de peticiones. Inténtelo de nuevo en unos segundos.";
            }
            else if (status == 429)
            {
                mensajeUsuario = "No se ha podido utilizar temporalmente el servicio de inteligencia artificial.";
            }
            else if (status == 500)
            {
                mensajeUsuario = "Se ha producido un error interno en el servicio de inteligencia artificial.";
            }
            else if (status == 502)
            {
                mensajeUsuario = "El servicio de inteligencia artificial no está disponible temporalmente.";
            }
            else if (status == 503)
            {
                mensajeUsuario = "El servicio de inteligencia artificial se encuentra temporalmente saturado.";
            }
            else if (status == 504)
            {
                mensajeUsuario = "La inteligencia artificial ha tardado demasiado tiempo en responder.";
            }

            return new RetornoBase(false, mensajeUsuario, detalleTecnico);
        }

        private async Task<RetornoBase> SubirFichero(string param_ruta_fichero)
        {
            try
            {
                OpenAIClient cliente = new OpenAIClient(Token);
                // Crear cliente de archivos
                OpenAIFileClient cliente_de_ficheros = cliente.GetOpenAIFileClient();

                // Subir el fichero
                OpenAIFile archivo = await cliente_de_ficheros.UploadFileAsync(param_ruta_fichero, FileUploadPurpose.Assistants);

                // Devolver el Id del fichero
                return new RetornoBase(true, "Fichero subido correctamente.", archivo.Id);
            }
            catch (ClientResultException e)
            {
                return ControlarExcepcion(e);
            }
        }
    }
}
