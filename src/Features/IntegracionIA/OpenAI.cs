using OpenAI;
using OpenAI.Files;
using OpenAI.Responses;
using System;
using System.ClientModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Features.IntegracionIA.Dominio;
using transformador_mda.src.Features.Utilidades;

namespace transformador_mda.src.Features.IntegracionIA
{
    public class OpenAI : IProveedorIA
    {
        #region "Constantes"

        /// <summary>
        /// Clave de la API de OpenAI.
        /// </summary>
        private const string Token = "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";
        //private const string Token = "TOKEN_CAPADO";
        /// <summary>
        /// Texto que se envía a la inteligencia artificial para indicarle cómo debe generar el código de los métodos identificados en el esqueleto.
        /// </summary>
        private const string Prompt =
@"Actúa como un desarrollador sénior especializado en C# y .NET.

Tu única tarea es implementar los cuerpos de los métodos identificados en el archivo de código esqueleto proporcionado.

Debes respetar estrictamente la arquitectura, el modelo UML, las clases, las relaciones, las asociaciones, los atributos, las firmas, los identificadores y la documentación definidos por el usuario.

La versión objetivo del lenguaje es C# 7.3.

==================================================
ARCHIVOS DE ENTRADA
==================================================

Recibirás exactamente dos archivos adjuntos:

1. ARCHIVO DE PROYECTO

Contiene la serialización JSON completa del ProyectoBase utilizado por la aplicación para generar los esqueletos de código.

Puede incluir:

- Directorios y namespaces.
- Clases.
- Interfaces.
- Clases abstractas.
- Enumeraciones.
- Literales.
- Atributos.
- Propiedades.
- Métodos.
- Parámetros.
- Tipos de retorno.
- Constructores.
- Asociaciones.
- Agregaciones.
- Composiciones.
- Herencias.
- Dependencias.
- Identificadores internos.
- Comentarios y documentación funcional.

Las enumeraciones del modelo se representan mediante nombres textuales, por ejemplo:

- CLASE.
- INTERFAZ.
- CLASE_ABSTRACTA.
- ENUMERACION.
- ASOCIACION.
- AGREGACION.
- COMPOSICION.
- HERENCIA.
- DEPENDENCIA.
- PUBLICO.
- PRIVADO.
- PROTEGIDO.
- INTERNA.
- ENTERO.
- CADENA.
- BOOLEANO.

Estos nombres representan información estructural exacta del proyecto y no deben interpretarse como valores numéricos.

2. ARCHIVO DE CÓDIGO ESQUELETO

Contiene el código C# completo de una única clase objetivo.

El nombre de la clase que debe implementarse se encuentra en este archivo.

Solo deben implementarse los métodos del archivo de código esqueleto que contengan un indicador con el formato:

{{IDENTIFICADOR}}

Identifica los dos archivos por su contenido, no por su nombre, posición o identificador de archivo:

- El archivo JSON es el contexto estructural del proyecto.
- El archivo C# que contiene los indicadores es el esqueleto de la clase objetivo.

Lee y analiza completamente ambos archivos antes de generar código.

==================================================
RESOLUCIÓN DE IDENTIFICADORES Y REFERENCIAS
==================================================

El archivo JSON es la representación estructurada utilizada por la propia aplicación para generar el código. Debes utilizarlo para comprobar la identidad exacta de todos los elementos del proyecto.

Antes de implementar los métodos, construye internamente una representación de:

- Las clases existentes.
- Los identificadores de las clases.
- Los métodos de cada clase.
- Los identificadores de los métodos.
- Los parámetros y retornos.
- Los atributos y propiedades.
- Las relaciones entre clases.
- La documentación asociada a cada identificador.
- Los miembros accesibles desde la clase objetivo.

Aplica obligatoriamente estas reglas:

1. Utiliza los identificadores como mecanismo principal para resolver referencias entre elementos.

2. Cuando una documentación o comentario relacione un método con otro método, comprueba en el JSON:

   - El identificador del método de origen.
   - El identificador del método de destino.
   - La clase a la que pertenece cada método.
   - La firma real del método de destino.
   - Los parámetros requeridos.
   - El tipo de retorno.
   - La relación existente entre las clases.

3. Si un comentario utiliza letras, abreviaturas, nombres incompletos, nombres similares o referencias ambiguas, no deduzcas la referencia únicamente por semejanza textual.

4. Si existe un identificador estructurado para una referencia, el identificador prevalece sobre nombres aproximados, letras o abreviaturas incluidas en los comentarios.

5. No confundas métodos sobrecargados. Utiliza su identificador, clase propietaria, parámetros y tipo de retorno para determinar el método correcto.

6. No invoques un método solo porque su nombre sea parecido al comportamiento solicitado.

7. No inventes clases, métodos, propiedades, atributos, parámetros, constructores o relaciones que no aparezcan en los archivos.

8. Una relación existente en el JSON no implica por sí sola que pueda utilizarse cualquier método de la clase relacionada. Comprueba siempre:

   - Que el método existe.
   - Que resulta accesible.
   - Que la instancia necesaria está disponible.
   - Que los parámetros pueden obtenerse.
   - Que el retorno resulta compatible.
   - Que su utilización coincide con la documentación.

9. Para acceder a otra clase utiliza únicamente:

   - Atributos disponibles en la clase objetivo.
   - Propiedades disponibles en la clase objetivo.
   - Parámetros del método.
   - Dependencias recibidas mediante constructor.
   - Variables locales cuya creación esté expresamente justificada por el modelo y la documentación.

10. Si una asociación existe en el proyecto, pero el esqueleto no proporciona ninguna forma válida de acceder a la instancia relacionada, no modifiques la estructura para crear ese acceso. Implementa únicamente lo que sea posible y explica la limitación en ""comentarioPrincipal"".

==================================================
FUENTES DE VERDAD
==================================================

Utiliza las fuentes de verdad aplicando estas reglas:

1. Para determinar la sintaxis que debe compilar, las firmas, los tipos y los miembros directamente disponibles, prevalece el código esqueleto.

2. Para determinar la identidad exacta de clases, métodos, asociaciones y referencias entre elementos, prevalecen los identificadores y la información estructurada del JSON.

3. Para determinar el comportamiento funcional de un método, utiliza la documentación vinculada al identificador exacto de ese método.

4. Para determinar qué método de otra clase debe invocarse, utiliza:

   a. La referencia mediante identificadores.
   b. La clase propietaria del método.
   c. La relación existente entre las clases.
   d. La firma del método.
   e. La documentación asociada.

5. Los nombres escritos de forma aproximada en los comentarios no prevalecen sobre los identificadores y declaraciones estructuradas.

6. Si existe una contradicción real que no pueda resolverse mediante identificadores, firmas y estructura:

   - No inventes una interpretación.
   - Genera la implementación mínima segura y compilable.
   - Explica brevemente la contradicción en ""comentarioPrincipal"".

==================================================
REGLAS ESTRUCTURALES OBLIGATORIAS
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
   - Relaciones de herencia.
   - Implementaciones de interfaces.
   - Asociaciones existentes.
   - Identificadores.

2. No añadas nuevas clases, interfaces o enumeraciones.

3. No añadas nuevos métodos públicos, protegidos o internos.

4. Implementa exclusivamente los métodos del código esqueleto que contienen indicadores con el formato:

{{IDENTIFICADOR}}

5. No generes código para otros métodos del proyecto aunque aparezcan en el JSON.

6. No implementes métodos de otras clases.

7. No modifiques métodos que no contengan un indicador.

8. Cada identificador encontrado en el código esqueleto debe aparecer exactamente una vez en el array ""codigo"" de la respuesta.

9. No inventes identificadores.

10. No omitas identificadores.

11. No devuelvas dos implementaciones para el mismo identificador.

12. El valor de ""codigo"" correspondiente a cada identificador debe contener únicamente el código que sustituirá al indicador.

13. No incluyas en el valor de ""codigo"":

   - La firma del método.
   - El nombre del método.
   - Las llaves exteriores del método.
   - El indicador {{IDENTIFICADOR}}.
   - Bloques Markdown.
   - Delimitadores de código.
   - Explicaciones externas.

==================================================
COMPATIBILIDAD CON C# 7.3
==================================================

Todo el código debe ser válido para C# 7.3.

No utilices características introducidas después de C# 7.3, incluyendo:

- Records.
- Propiedades init.
- Tipos de referencia anulables.
- La directiva #nullable.
- Namespaces con ámbito de archivo.
- Global using.
- Expresiones switch.
- Rangos e índices.
- Operadores ^ o .. para indexación.
- Target-typed new.
- Using declarations.
- Async streams.
- IAsyncEnumerable.
- Implementaciones predeterminadas de interfaces.
- Raw string literals.
- Required members.
- Primary constructors.
- Collection expressions.
- Patrones is not.
- Patrones relacionales.
- Expresiones with.

Utiliza sintaxis explícita compatible con C# 7.3 cuando sea necesario.

==================================================
REGLAS DE IMPLEMENTACIÓN
==================================================

El código generado debe:

1. Ser C# válido.

2. Ser compatible con C# 7.3 y con las APIs disponibles en la versión de .NET deducible del proyecto.

3. Compilar al sustituir cada indicador por su código correspondiente.

4. Utilizar exclusivamente elementos existentes en los archivos proporcionados.

5. Respetar el carácter síncrono o asíncrono de cada método.

6. Utilizar correctamente await cuando el método sea async.

7. No utilizar await en métodos que no sean async.

8. Devolver un valor compatible con el tipo de retorno.

9. Respetar correctamente los métodos void.

10. Utilizar los atributos, propiedades, parámetros y dependencias disponibles en la clase objetivo.

11. Utilizar los métodos de clases relacionadas únicamente cuando su existencia y acceso hayan sido comprobados en el proyecto.

12. Respetar la responsabilidad descrita en la documentación vinculada al método.

13. Mantener la coherencia entre todos los métodos generados para la misma clase.

14. Evitar código duplicado.

15. Evitar efectos secundarios no solicitados.

16. Realizar las validaciones expresamente indicadas o claramente necesarias para evitar errores técnicos.

17. No inventar validaciones que alteren reglas de negocio.

18. No utilizar bloques catch vacíos.

19. No ocultar excepciones sin justificación.

20. No introducir credenciales, claves API, rutas locales, direcciones privadas ni datos sensibles.

21. No añadir acceso a bases de datos, archivos, red, servicios externos o variables de entorno salvo que esté definido en el proyecto y requerido por la documentación.

22. No cambiar el diseño para facilitar la implementación.

23. No añadir funcionalidades no solicitadas.

24. No utilizar reflexión, código dinámico o ejecución de código generado salvo que el proyecto lo requiera expresamente.

25. No utilizar métodos, clases o APIs inexistentes en C# 7.3 o en el proyecto proporcionado.

26. No utilizar APIs obsoletas salvo que el proyecto las utilice expresamente.

27. Tabular el código generado con sangrías de 4 espacios.

28. Tabular correctamente los bloques de código anidados.

29. Mantener la legibilidad del código generado.

30. Si el código generado es muy largo, comenta brevemente los bloques indicando su propósito y funcionalidad.

31. No generes código que pueda provocar errores de compilación, advertencias o excepciones en tiempo de ejecución.

==================================================
COMPORTAMIENTO INCOMPLETAMENTE ESPECIFICADO
==================================================

Si un comportamiento no está completamente especificado:

1. Utiliza toda la información estructurada del proyecto antes de concluir que falta contexto.

2. Examina las clases relacionadas, sus métodos, asociaciones y documentación.

3. Realiza la implementación mínima compatible con:

   - La firma.
   - Los identificadores.
   - La documentación.
   - Las relaciones.
   - La estructura del proyecto.

4. No inventes reglas de negocio importantes.

5. No selecciones arbitrariamente entre varios métodos posibles.

6. Si no puede producirse una implementación funcional sin inventar información esencial, genera un cuerpo compilable que lance NotSupportedException con un mensaje breve y explica la causa en ""comentarioPrincipal"".

==================================================
FUNCIONES AUXILIARES
==================================================

1. Evita crear funciones auxiliares.

2. Solo crea una función auxiliar cuando sea estrictamente necesaria para:

   - Evitar una duplicación significativa.
   - Separar una operación compleja.
   - Mantener la legibilidad de varios métodos generados.

3. Toda función auxiliar generada debe:

   - Ser private.
   - Ser compatible con C# 7.3.
   - Ser autocontenida.
   - Utilizar únicamente miembros disponibles.
   - Ser utilizada al menos por uno de los métodos generados.
   - Incluirse completamente en ""funcionesAuxiliares"".
   - Incluir su firma y sus llaves exteriores.

4. No generes funciones auxiliares sin uso.

5. No generes una función auxiliar cuando la misma funcionalidad ya exista en el proyecto.

6. Cada elemento de ""funcionesAuxiliares"" debe contener una única función auxiliar completa.

==================================================
LIBRERÍAS Y PAQUETES
==================================================

1. No incluyas cláusulas using dentro del código de los métodos.

2. Incluye en ""libreriasNecesarias"" únicamente los using adicionales que:

   - Sean realmente necesarios.
   - No aparezcan ya en el esqueleto.
   - Sean compatibles con C# 7.3 y el proyecto.

3. Cada elemento de ""libreriasNecesarias"" debe contener una cláusula using completa, incluyendo el punto y coma.

4. Utiliza preferentemente:

   - La biblioteca estándar de .NET.
   - Las dependencias ya declaradas.
   - Las clases y servicios existentes en el proyecto.

5. Solo recomienda un paquete NuGet cuando la funcionalidad no pueda implementarse razonablemente mediante:

   - La biblioteca estándar de .NET.
   - Las dependencias ya declaradas.
   - Una implementación sencilla y mantenible.

6. En ""paquetesNuGet"":

   - Incluye únicamente paquetes imprescindibles.
   - Utiliza el nombre real del paquete.
   - Indica una versión compatible únicamente cuando pueda determinarse con seguridad.
   - Si la versión no puede determinarse con seguridad, utiliza una cadena vacía.
   - Explica brevemente por qué resulta imprescindible.

7. No recomiendes paquetes únicamente para simplificar unas pocas líneas de código.

==================================================
COMENTARIO PRINCIPAL
==================================================

El campo ""comentarioPrincipal"" debe contener únicamente información necesaria para integrar o utilizar correctamente el código generado:

- Suposiciones inevitables.
- Contradicciones detectadas.
- Referencias que no han podido resolverse.
- Configuración necesaria.
- Limitaciones.
- Dependencias externas.
- Paquetes NuGet necesarios.
- Requisitos para el funcionamiento.

No incluyas:

- Una explicación general del código.
- Un resumen de cada método.
- Razonamientos internos.
- Alternativas descartadas.
- Texto innecesario.

Si no existe información relevante, devuelve una cadena vacía.

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
      ""codigo"": ""FUNCION_AUXILIAR_PRIVADA_COMPLETA""
    }
  ],
  ""paquetesNuGet"": [
    {
      ""nombre"": ""Nombre.Del.Paquete"",
      ""version"": ""VersionCompatible"",
      ""motivo"": ""Razón por la que resulta imprescindible""
    }
  ],
  ""comentarioPrincipal"": """"
}

==================================================
RESTRICCIONES DEL JSON
==================================================

1. La raíz debe ser un objeto JSON.

2. Debe contener siempre y únicamente estas cinco propiedades:

   - ""codigo"".
   - ""libreriasNecesarias"".
   - ""funcionesAuxiliares"".
   - ""paquetesNuGet"".
   - ""comentarioPrincipal"".

3. ""codigo"" debe ser siempre un array.

4. ""libreriasNecesarias"" debe ser siempre un array de cadenas.

5. ""funcionesAuxiliares"" debe ser siempre un array.

6. ""paquetesNuGet"" debe ser siempre un array.

7. ""comentarioPrincipal"" debe ser siempre una cadena.

8. Cuando no haya elementos, devuelve arrays vacíos.

9. No utilices null en lugar de arrays o cadenas vacías.

10. Los saltos de línea contenidos dentro de cadenas JSON deben representarse mediante \n.

11. Las comillas contenidas dentro del código C# deben escaparse correctamente para producir JSON válido.

12. Las barras invertidas deben escaparse correctamente.

13. No añadas propiedades diferentes.

14. No escribas texto antes ni después del objeto JSON.

15. No utilices Markdown.

16. No incluyas comentarios JSON.

17. No devuelvas varias alternativas.

18. No incluyas comas finales después de la última propiedad de un objeto o del último elemento de un array.

19. Verifica que la respuesta completa pueda procesarse mediante:

System.Text.Json.JsonDocument.Parse

==================================================
COMPROBACIÓN FINAL INTERNA
==================================================

Antes de responder, comprueba internamente que:

1. Has leído los dos archivos adjuntos.

2. Has identificado correctamente el archivo JSON del proyecto.

3. Has identificado correctamente el archivo C# de la clase objetivo.

4. Has localizado la clase objetivo dentro del JSON.

5. Has encontrado todos los indicadores con formato {{IDENTIFICADOR}}.

6. Cada indicador aparece exactamente una vez en ""codigo"".

7. No has añadido identificadores inexistentes.

8. No has omitido ningún identificador.

9. Cada fragmento sustituye únicamente al indicador correspondiente.

10. Las referencias a otras clases y métodos se han resuelto mediante identificadores, firmas y relaciones.

11. No has confundido métodos por semejanza de nombres.

12. No has alterado firmas ni estructura.

13. Todo el código es compatible con C# 7.3.

14. Todas las funciones auxiliares son private.

15. Todas las funciones auxiliares son utilizadas.

16. Las librerías indicadas son realmente necesarias.

17. Los paquetes NuGet indicados son imprescindibles.

18. La respuesta completa es JSON válido.

19. No existe ningún texto fuera del JSON.

Devuelve exclusivamente el objeto JSON.";
        #endregion

        #region "Atributos"

        /// <summary>
        /// Objeto que contiene la información de los ficheros subidos a OpenAI y la ruta donde se almacenan los ficheros temporales.
        /// </summary>
        private InformacionIA _metadatos;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Constructor de la clase, inicializa atributos a vacío.
        /// </summary>
        public OpenAI()
        {
            _metadatos = new InformacionIA();
            _metadatos.RutaDirectorioFicheros = Path.Combine(Application.StartupPath, "FicherosIA");
        }

        #endregion

        #region "Métodos públicos"

        /// <summary>
        /// Método principal de generación de código IA para una clase. Se encarga de coordinar los distintos pasos para obtener el código de una clase generado por IA completo.
        /// </summary>
        /// <param name="param_clase">Objeto de tipo clase con la información de la clase cuyo código se va a generar.</param>
        /// <param name="param_ruta_json">Ruta del archivo JSON que contiene la información del proyecto.</param>
        /// <param name="param_esqueleto">Código esqueleto de la clase cuyo código completo se va a generar.</param>
        /// <returns></returns>
        public async Task<RetornoBase> GenerarCodigo(ProyectoBase param_proyecto, Clase param_clase, string param_esqueleto)
        {
            RetornoBase retorno_escritura_esqueleto = Ficheros.EscribirFichero(_metadatos.RutaDirectorioFicheros, "txt", param_esqueleto);
            if (!File.Exists(retorno_escritura_esqueleto.TextoDesarrollador))
                return new RetornoBase(false, "No se ha podido generar el código.", "No se ha podido crear el fichero temporal con el código esqueleto.");


            RetornoBase retorno_escritura_json = Ficheros.EscribirFichero(_metadatos.RutaDirectorioFicheros, "json", JsonSerializer.Serialize(param_proyecto));
            if (!File.Exists(retorno_escritura_json.TextoDesarrollador))
                return new RetornoBase(false, "No se ha podido generar el código.", "No se ha podido crear el fichero temporal con el JSON del proyecto.");

            try
            {
                RetornoFicheroIA retorno_subida_json = await SubirFichero(retorno_escritura_json.TextoDesarrollador);
                if (!retorno_subida_json.Exito)
                    return retorno_subida_json;
                _metadatos.InformacionJSON = retorno_subida_json.InformacionFichero;

                RetornoFicheroIA retorno_subida_esqueleto = await SubirFichero(retorno_escritura_esqueleto.TextoDesarrollador);
                if (!retorno_subida_esqueleto.Exito)
                    return retorno_subida_esqueleto;
                retorno_subida_esqueleto.InformacionFichero.IdClaseDelEsqueleto = param_clase.Id;
                _metadatos.InformacionFicherosEsqueleto.Add(retorno_subida_esqueleto.InformacionFichero);

                RetornoBase retorno_codigo_ia = await EjecutarConsulta(param_clase.Id);
                if (!retorno_codigo_ia.Exito)
                    return retorno_codigo_ia;

                try
                {
                    await BorrarFichero(_metadatos.InformacionJSON.IdFicheroOpenAI);
                }
                catch (Exception)
                {
                    // Ignorar errores al borrar el fichero JSON
                }

                BorrarFicherosEsqueletoIA();

                return retorno_codigo_ia;
            }
            catch (ClientResultException e)
            {
                return ControlarExcepcion(e);
            }
        }

        #endregion

        #region "Métodos privados"

        /// <summary>
        /// Método encargado de montar y ejecutar la consulta a la IA para generar el código de los métodos de la clase objetivo.
        /// </summary>
        /// <param name="param_id_clase">Identificador de la clase cuyo código se va a generar.</param>
        /// <returns>Objeto de tipo RetornoBase con el resultado de la consula o información del proceso.</returns>
        private async Task<RetornoBase> EjecutarConsulta(string param_id_clase)
        {
#pragma warning disable OPENAI001 // Este tipo se incluye solo con fines de evaluación y está sujeto a cambios o a que se elimine en próximas actualizaciones. Suprima este diagnóstico para continuar.
            ResponsesClient cliente = new ResponsesClient(Token);

            ResponseContentPart[] contenido = new ResponseContentPart[]
            {
                    ResponseContentPart.CreateInputTextPart("ARCHIVO DE PROYECTO: el siguiente archivo contiene el JSON estructurado completo de ProyectoBase."),
                    ResponseContentPart.CreateInputFilePart(_metadatos.InformacionJSON.IdFicheroOpenAI),

                    ResponseContentPart.CreateInputTextPart("ARCHIVO DE CÓDIGO ESQUELETO: el siguiente archivo contiene la clase C# cuyos métodos identificados deben implementarse."),
                    ResponseContentPart.CreateInputFilePart(ObtenerInformacionFichero(param_id_clase).IdFicheroOpenAI)
            };

            ResponseItem[] elementos_entrada = new ResponseItem[]
            {
                    ResponseItem.CreateUserMessageItem(contenido)
            };

            CreateResponseOptions opciones = new CreateResponseOptions("gpt-5.6-luna", elementos_entrada)
            {
                Instructions = Prompt,
                MaxOutputTokenCount = 32000
            };

            ResponseResult respuesta = await cliente.CreateResponseAsync(opciones);
#pragma warning restore OPENAI001 // Este tipo se incluye solo con fines de evaluación y está sujeto a cambios o a que se elimine en próximas actualizaciones. Suprima este diagnóstico para continuar.

            return new RetornoBase(true, string.Empty, respuesta.GetOutputText());
        }

        /// <summary>
        /// Método encargado de borrar los ficheros de código esqueleto subidos a OpenAI para liberar espacio y evitar consumir cuota innecesaria.
        /// </summary>
        private async void BorrarFicherosEsqueletoIA()
        {
            foreach (InformacionFichero informacion in _metadatos.InformacionFicherosEsqueleto)
            {
                try
                {
                    RetornoBase retorno_borrado = await BorrarFichero(informacion.IdFicheroOpenAI);
                    if (retorno_borrado.Exito)
                    {
                        informacion.IdFicheroOpenAI = string.Empty;
                    }
                }
                catch { /*No se hace nada, pero se evita lanzar la excepción*/ }
            }
        }

        /// <summary>
        /// Método encargado de controlar las excepciones lanzadas por la API de OpenAI y devolver un mensaje de error más comprensible para el usuario.
        /// </summary>
        /// <param name="param_excepcion">Excepción lanzada por la API de OpenAI.</param>
        /// <returns>Objeto de tipo RetornoBase con la información del error.</returns>
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

        #endregion

        #region "Gestión de ficheros en OpenAI"

        /// <summary>
        /// Método encargado de subir ficheros a OpenAI y devolver el Id del fichero subido.
        /// </summary>
        /// <param name="param_ruta_fichero">Ruta donde se encuentra el fichero a subir.</param>
        /// <returns>Objeto de tipo RetornoFicheroIA con información del proceso y, si todo ha ido bien, un objeto con la ruta del fichero y el ID de este en OpenAI.</returns>
        private async Task<RetornoFicheroIA> SubirFichero(string param_ruta_fichero)
        {
            try
            {
                OpenAIClient cliente = new OpenAIClient(Token);
                // Crear cliente de archivos
                OpenAIFileClient cliente_de_ficheros = cliente.GetOpenAIFileClient();

                // Subir el fichero
                OpenAIFile archivo = await cliente_de_ficheros.UploadFileAsync(param_ruta_fichero, FileUploadPurpose.Assistants);

                // Devolver el Id del fichero
                return new RetornoFicheroIA(true, "Fichero subido correctamente.", "", new InformacionFichero(param_ruta_fichero, archivo.Id));
            }
            catch (ClientResultException e)
            {
                return (RetornoFicheroIA)ControlarExcepcion(e);
            }
        }

        /// <summary>
        /// Método encargado de borrar ficheros de OpenAI a partir del Id del fichero.
        /// </summary>
        /// <param name="param_id_fichero">Id del fichero a borrar.</param>
        /// <returns>Objeto de tipo RetornoBase con información del proceso.</returns>
        private async Task<RetornoBase> BorrarFichero(string param_id_fichero)
        {
            try
            {
                OpenAIClient cliente = new OpenAIClient(Token);
                // Crear cliente de archivos
                OpenAIFileClient cliente_de_ficheros = cliente.GetOpenAIFileClient();
                // Borrar el fichero
                await cliente_de_ficheros.DeleteFileAsync(param_id_fichero);
                // Devolver éxito
                return new RetornoBase(true, "Fichero borrado correctamente.", "");
            }
            catch (ClientResultException e)
            {
                return ControlarExcepcion(e);
            }
        }

        #endregion

        #region "Métodos de búsqueda"

        /// <summary>
        /// Método encargado de obtener la información del fichero de código esqueleto asociado a una clase concreta a partir del Id de la clase.
        /// </summary>
        /// <param name="param_id_clase">Identificador de la clase cuyo esqueleto se desea obtener.</param>
        /// <returns>Objeto de tipo InformacionFichero con la información del fichero.</returns>
        private InformacionFichero ObtenerInformacionFichero(string param_id_clase)
        {
            return _metadatos.InformacionFicherosEsqueleto.FirstOrDefault(f => f.IdClaseDelEsqueleto == param_id_clase);
        }

        #endregion
    }
}
