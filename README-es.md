# CommandLineParser
## Introducción

CommandLineParser es un biblioteca destinada a facilitar el uso de argumentos de línea de comando, reduciendo drásticamente la cantidad de código. CommandLineParser permite:

* Detectar y tipificar parámetros, asignándole propiedades como obligatoriedad, tipo de dato y valores por default.
* Soporte nativo para enumeraciones (enums) con parseo automático y mapeo personalizado de valores.
* Validarlos automáticamente en base a las propiedades asignadas, verificando su tipo de dato, formato, rango (si corresponde) y presencia.
* Alertar por medio de Exceptions especificas, las inconsistencias halladas.
* Promover a que las aplicaciones usen una única convención estandarizada de nombres y significados de parametrización alineadas con el estilo estándar de las aplicaciones de Linux (estilo mas difundido entre Linux y DOS. Ver The Art of UNIX Programming)
* Generar automáticamente la pantalla de ayuda listando los comandos aceptados por el programa, para ser invocada con alguno de los comandos estándar de Linux/DOS para el listado de ayuda.

Todo esto sin necesidad de escribir código de lógica de negocio ni validación, simplemente describiendo el comportamiento que se espera de cada parámetro por medio de _Attributes_.


***

### Tabla de contenido
* [¿Por que usar CommandLineParser?](https://github.com/tresvi/CommandLineParser?tab=readme-ov-file#por-qu%C3%A9-usar-commandlineparser)
* [Compatibilidad](https://github.com/tresvi/CommandLineParser?tab=readme-ov-file#compatibilidad)
* [Descripción de uso y funcionamiento](https://github.com/tresvi/CommandLineParser?tab=readme-ov-file#descripci%C3%B3n-de-uso-y-funcionamiento)
* [Terminología](https://github.com/tresvi/CommandLineParser?tab=readme-ov-file#terminolog%C3%ADa)
* [Próximas funcionalidades](https://github.com/tresvi/CommandLineParser?tab=readme-ov-file#pr%C3%B3ximas-funcionalidades)

### Ejemplos
* [Ejemplo básico](https://github.com/tresvi/CommandLineParser/wiki/Ejemplo-b%C3%A1sico)
* [Ejemplo Con Checkers y Formatters](https://github.com/tresvi/CommandLineParser/wiki/Ejemplo-con-uso-de-Checkers-y-Formatters)
  
***


### ¿Por qué usar CommandLineParser?
#### Simplificación  de código y disminución de probabilidad de errores
Todo programa de consola con cierta versatilidad necesita del uso de parámetros de invocación para comandar su uso. La correcta detección y completa validación de cada uno conlleva cierta cantidad de código que escala rápidamente si aumenta la cantidad de parámetros a utilizar, y aun mas si alguno de estos parámetros implica un "modo de funcionamiento especifico" del programa.
Los "modos de funcionamiento específicos" mencionados anteriormente, son llamados de forma común "verbos". 
Un ejemplo conocido de utilización de verbos es el del comando "git". Si bien el comando invoca al mismo programa, el juego de comandos validos para cada acción o verbo (commit, clone branch, checkout, etc...) varía ampliamente, por ejemplo:
* **git commit -m "Esto es un commit"**  Realiza un commit, y necesita una descripcion corta obligatoria
* **git branch**   Lista las ramas del repositorio pero no necesita argumento
* **git branch -d nombre-de-la-rama**  Borra un rama, por lo que el ahora el nombre de la rama es obligatorio

Como se puede ver, la aparición de verbos cambia totalmente el contexto de operación del programa volviendo en algunos casos indispensables a algunos parámetros que en otros modos no tienen utilidad o incluso no deberían estar presentes. Este cambio de modo de operación puede disparar la complejidad de detección y validación de parámetros forzando la aparición de grandes cantidades de código dedicado a esta tarea, y creando una lógica compleja de validaciones condicionales, todo esto ensuciando la lógica de negocio principal del programa. Dichas condiciones mencionadas, son muy propicias para la aparición bugs, lo cual también dispara el tiempo y la cantidad de pruebas a realizar sobre el programa.

#### Convención de uso y de nombres de parámetros
En caso de que cada programa implemente su propio manejo customizado de línea de comando, cada programador optará por alguno de las múltiples estilos estándar o inventados para el pasaje de parámetros, como ser posicional, nominal, nominal separado por un espacio, nominal separado un "=", nominal separado por un ":", y un largo etc. de posibilidades.
Al utilizar CommandLineParser, la interfase de la misma reduce el abanico de posibilidades, acercándonos a una de las convenciones de Linux mas usadas y la adoptada por todas las herramientas de .NET Core. De esta manera se evitan dudas y errores en su invocación.

#### Descripción detallada ante un fallo
En caso de que el usuario haya realizado una invocación con parámetros incorrectos al programa que utiliza la presente biblioteca, la excepción devuelta se lanzará tipificada de acuerdo a su naturaleza y con un mensaje claro y descriptivo sobre el problema encontrado a fin de que el usuario lo pueda corregir fácilmente.

#### Generación automática de pantalla de ayuda
Al utilizar esta biblioteca, la información para la generación de la pantalla de ayuda es recopilada automáticamente usando la información de la clase, por lo cual al invocar al programa en cuestión con alguno de los parámetros mas comunes para solicitar ayuda, se desplegará automáticamente la ayuda sin necesidad de programarla:
* miPrograma.exe -h
* miPRograma.exe --help
* miPrograma.exe /?
* miPrograma.exe -help 
La misma listará toda la lista de verbos detallando la descripción, el tipo y la obligatoriedad de cada parámetro.


## Compatibilidad
Al estar desarrollada bajo el standard [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0), la misma es compatible con los siguientes Runtimes

| Implementación | Versión del Framework |
| ------------- | ------------- |
| .NET Framework  | 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8  |
| .NET y .NET Core  | 2.0, 2.1, 2.2, 3.0, 3.1, 5.0, 6.0 y posteriores  |


## Descripción de uso y funcionamiento

Esencialmente el modo de uso se basa en la creación de una clase conteniendo _properties_ donde cada una representa los posibles parámetros que puede recibir un programa. La mismas se deberán decorar con _attributes_ para describir su comportamiento.
El principio de funcionamiento de esta biblioteca es el uso de técnicas de _Reflection_, por medio de las cuales el motor de validación y parsing accede a el tipo de la _property_ y a los atributos a fin de saber que características validar e inyectar en la instancia de salida.

## Terminología
_Options_: Se denominan así a lo que normalmente se conoce como parámetro. Puede ser obligatorio o no. Se deben especificar su nombre largo y su nombre corto (de solo una letra). Al usarlo en la linea de comando el nombre largo debe ir antecedido por un doble guión medio, el corto, por un guion medio simple. Ejemplo:
```
\>myPrograma.exe --inputfile "C:\Temp\input.txt"
```
o bien
```
\>myPrograma.exe -i "C:\Temp\input.txt"
```

Las Options pueden ser de cualquier tipo primitivo, tipos nullable, DateTime, enumeraciones (enums), o strings.

_Flags_: Son parámetros que no llevan valor, ni son obligatortios. El flag es simplemente un parámetro que cuando se incluye en la línea de comando, su property asociada se setea en _true_, cuando no aparece, se setea en _false_. Por esta razón siempre se los debe asociar con variables boolean. Ejemplo:

```
\>delete.exe recursive
```
o bien
```
\>delete.exe
```

_Verbs_: Son indicadores de como se comportará el programa y se asocian a como o en que modo se comportará el programa. Cada verbo tiene una clase asociada (no una property como las Options y los Flags) y dentro de ella, puede tener properties asociadas a mas Options y Flags. Ejemplo:
```
\>git commit -m "Esto es un commit"
```
o bien
```
\>git add -A
```
En los anteriores ejemplos, tenemos los verbos _commit_ y _add_. El primero tiene asociada la Option "-m" y el segundo el  el flag "-A"

Nota: Todos los terminos mencionados son **case sensitive**.

## Soporte de Enumeraciones (Enums)

CommandLineParser proporciona soporte nativo para tipos enumeración con parseo automático y mapeo personalizado opcional.

### Uso Básico de Enums

Los enums se detectan y parsean automáticamente. Puedes usar nombres del enum (sin distinción de mayúsculas/minúsculas) o índices numéricos:

```csharp
public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error
}

public class Parametros
{
    [Option("level", 'l', false, "Nivel de logging")]
    public LogLevel LogLevel { get; set; }
}
```

**Uso:**
```bash
miPrograma.exe --level Info
miPrograma.exe --level info        # Sin distinción de mayúsculas/minúsculas
miPrograma.exe --level 1           # Índice numérico
```

### Enums Nullable

Los enums nullable están completamente soportados para parámetros opcionales:

```csharp
public enum Ambiente
{
    Desarrollo,
    Staging,
    Produccion
}

public class Parametros
{
    [Option("env", 'e', false, "Ambiente objetivo")]
    public Ambiente? Ambiente { get; set; }
}
```

### Mapeo Personalizado de Enums

Usa `[EnumMap]` para definir valores de entrada personalizados que se mapean a valores del enum. Esto es útil para aliases o cuando quieres aceptar nombres diferentes a los miembros del enum:

```csharp
public enum FormatoSalida
{
    Json,
    Xml,
    Csv,
    Yaml
}

public class Parametros
{
    [Option("formato", 'f', false, "Formato de salida")]
    [EnumMap("json", FormatoSalida.Json)]
    [EnumMap("xml", FormatoSalida.Xml)]
    [EnumMap("csv", FormatoSalida.Csv)]
    [EnumMap("j", FormatoSalida.Json)]  // Alias corto
    [EnumMap("x", FormatoSalida.Xml)]   // Alias corto
    public FormatoSalida Formato { get; set; }
}
```

**Uso:**
```bash
miPrograma.exe --formato json
miPrograma.exe --formato j           # Alias corto
miPrograma.exe --formato JSON          # Sin distinción de mayúsculas/minúsculas
```

**Nota:** Cuando se usa `[EnumMap]`, solo se aceptan los valores mapeados. El parseo automático de nombres del enum se desactiva para esa propiedad.

### Enums con Valores Numéricos Personalizados

Los enums con valores numéricos personalizados están completamente soportados:

```csharp
public enum Prioridad
{
    Baja = 1,
    Media = 5,
    Alta = 10,
    Critica = 99
}

public class Parametros
{
    [Option("prioridad", 'p', false, "Nivel de prioridad")]
    public Prioridad Prioridad { get; set; }
}
```

**Uso:**
```bash
miPrograma.exe --prioridad Alta
miPrograma.exe --prioridad 10        # Valor numérico
miPrograma.exe --prioridad 1         # Valor numérico
```

### Combinando Enums con Validación

Puedes combinar propiedades enum con `[EnumeratedValidation]` para restringir qué valores del enum se aceptan:

```csharp
public class Parametros
{
    [Option("level", 'l', false, "Nivel de logging")]
    [EnumeratedValidation(new[] { "Debug", "Info", "Warning" })]
    public LogLevel LogLevel { get; set; }
}
```

Esto restringe el `LogLevel` a solo `Debug`, `Info`, o `Warning` (rechazando `Error`).

## Próximas funcionalidades
...

[Sobre este repositorio]()
***

## Links y bibliografía de interes:

* The Art of UNIX Programming - Libro de Eric S. Raymond
* [https://pubs.opengroup.org/onlinepubs/9699919799/basedefs/V1_chap12.html](https://pubs.opengroup.org/onlinepubs/9699919799/basedefs/V1_chap12.html)
* [https://tldp.org/LDP/abs/html/standard-options.html](https://tldp.org/LDP/abs/html/standard-options.html)
* [https://www.gnu.org/prep/standards/html_node/Command_002dLine-Interfaces.html](https://www.gnu.org/prep/standards/html_node/Command_002dLine-Interfaces.html)
* [https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run)



