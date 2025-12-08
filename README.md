# CommandLineParser

A powerful and intuitive .NET library for parsing command-line arguments with minimal code. CommandLineParser simplifies command-line interface development by using attributes to define parameters, their validation rules, and automatic help generation.

## Features

* **Declarative Parameter Definition**: Define parameters using attributes on class properties
* **Automatic Type Conversion**: Supports all primitive types, nullable types, DateTime, and custom formatting
* **Built-in Validation**: Comprehensive validation attributes for common scenarios (email, IP addresses, file/directory existence, regex patterns, enumerated values)
* **Verb Support**: Implement Git-like verb commands (e.g., `git commit`, `git push`) with separate parameter sets per verb
* **Default Verbs**: Define a default verb that executes when no verb is specified
* **Parameter Dependencies**: Define required and incompatible parameter relationships
* **Automatic Help Generation**: Generate help text automatically using `--help`, `-h`, `/?`, or `-help`
* **Specific Exceptions**: Typed exceptions with clear, descriptive error messages for easy troubleshooting
* **Standard Conventions**: Follows Linux/Unix command-line conventions, compatible with .NET Core CLI tools

## Compatibility

Built on [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0), CommandLineParser is compatible with:

| Implementation | Framework Version |
| -------------- | ----------------- |
| .NET Framework | 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8 |
| .NET and .NET Core | 2.0, 2.1, 2.2, 3.0, 3.1, 5.0, 6.0 and later |

## Installation

Install via NuGet:

```bash
dotnet add package Tresvi.CommandLineParser
```

Or using Package Manager:

```powershell
Install-Package Tresvi.CommandLineParser
```

## Quick Start

### Basic Example

```csharp
using Tresvi.CommandParser;
using Tresvi.CommandParser.Attributtes.Keywords;

public class Parameters
{
    [Option("inputfile", 'i', true, "Input file to process")]
    public string InputFile { get; set; }

    [Option("outputfile", 'o', false, "Output file path")]
    public string OutputFile { get; set; }

    [Flag("verbose", 'v', "Enable verbose output")]
    public bool Verbose { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Parameters parameters = CommandLine.Parse<Parameters>(args);
            
            Console.WriteLine($"Processing: {parameters.InputFile}");
            if (parameters.Verbose)
                Console.WriteLine("Verbose mode enabled");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

**Usage:**
```bash
myProgram.exe --inputfile "C:\data\input.txt" --outputfile "C:\data\output.txt" --verbose
# or
myProgram.exe -i "C:\data\input.txt" -o "C:\data\output.txt" -v
```

### Verbs Example (Git-like Commands)

```csharp
using Tresvi.CommandParser;
using Tresvi.CommandParser.Attributtes.Keywords;

[Verb("commit", "Record changes to the repository")]
public class Commit
{
    [Option("message", 'm', true, "Commit message")]
    public string Message { get; set; }

    [Flag("amend", 'a', "Amend previous commit")]
    public bool Amend { get; set; }
}

[Verb("push", "Update remote refs along with associated objects", isDefault: true)]
public class Push
{
    [Option("remote", 'r', false, "Remote repository name")]
    public string Remote { get; set; } = "origin";
}

class Program
{
    static void Main(string[] args)
    {
        try
        {
            object result = CommandLine.Parse(args, typeof(Commit), typeof(Push));
            
            if (result is Commit commit)
            {
                Console.WriteLine($"Committing: {commit.Message}");
            }
            else if (result is Push push)
            {
                Console.WriteLine($"Pushing to: {push.Remote}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

**Usage:**
```bash
myProgram.exe commit -m "Initial commit"
myProgram.exe push --remote upstream
myProgram.exe --remote upstream  # Uses default verb (push)
```

## Terminology

### Options
Parameters that accept a value. Can be required or optional. Use long name (prefixed with `--`) or short name (prefixed with `-`).

**Example:**
```bash
myProgram.exe --inputfile "C:\Temp\input.txt"
# or
myProgram.exe -i "C:\Temp\input.txt"
```

### Flags
Boolean parameters that don't require a value. When present, the property is set to `true`; otherwise, `false`.

**Example:**
```bash
myProgram.exe --recursive
# or
myProgram.exe -r
```

### Verbs
Commands that change the program's behavior mode. Each verb has its own class with associated Options and Flags. Similar to Git commands (`git commit`, `git push`, etc.).

**Example:**
```bash
git commit -m "This is a commit"
git add -A
```

In these examples, `commit` and `add` are verbs. The first has an Option `-m`, and the second has a Flag `-A`.

**Note:** All terms are **case-sensitive**.

## Validation Attributes

CommandLineParser provides several built-in validation attributes:

### File and Directory Validation
- `[FileExists]` - Validates that a file exists
- `[FileNotExists]` - Validates that a file does not exist
- `[DirectoryExists]` - Validates that a directory exists
- `[DirectoryNotExists]` - Validates that a directory does not exist

### Format Validation
- `[EmailValidation]` - Validates email address format
- `[IPValidation]` - Validates IP addresses (IPv4/IPv6, with optional port)
- `[RegexValidation]` - Validates using regular expressions
- `[EnumeratedValidation]` - Validates against a list of allowed values

### Example with Validation

```csharp
public class Parameters
{
    [FileExists]
    [Option("config", 'c', true, "Configuration file path")]
    public string ConfigFile { get; set; }

    [EmailValidation]
    [Option("email", 'e', true, "Administrator email")]
    public string Email { get; set; }

    [EnumeratedValidation(new[] { "dev", "test", "prod" }, caseSensitive: false)]
    [Option("environment", 'e', false, "Environment (dev, test, prod)")]
    public string Environment { get; set; }
}
```

## Parameter Relationships

### Required Parameters
Define that one parameter requires another:

```csharp
[Option("output", 'o', false, "Output file")]
[Requires(nameof(Input))]
public string Output { get; set; }

[Option("input", 'i', false, "Input file")]
public string Input { get; set; }
```

### Incompatible Parameters
Define that parameters cannot be used together:

```csharp
[Option("output", 'o', false, "Output file")]
[IncompatibleWith(nameof(Overwrite), nameof(Append))]
public string Output { get; set; }

[Flag("overwrite", 'w', "Overwrite existing file")]
[IncompatibleWith(nameof(Append))]
public bool Overwrite { get; set; }

[Flag("append", 'a', "Append to file")]
public bool Append { get; set; }
```

## Exception Handling

CommandLineParser throws specific exceptions for different error scenarios:

- `UnknownParameterException` - Unknown parameter provided
- `RequiredParameterNotFoundException` - Required parameter missing
- `ValueNotFoundException` - Parameter value missing
- `MultiInvocationParameterException` - Parameter used multiple times
- `MultiDefinitionParameterException` - Duplicate parameter definition
- `InvalidFormatException` - Format validation failed
- `InvalidEmailAddressException` - Email validation failed
- `InvalidIPAddressException` - IP validation failed
- `IncompatibleParametersException` - Incompatible parameters used together
- And more...

**Example:**
```csharp
try
{
    Parameters parameters = CommandLine.Parse<Parameters>(args);
}
catch (RequiredParameterNotFoundException ex)
{
    Console.WriteLine($"Missing required parameter: {ex.Message}");
}
catch (InvalidEmailAddressException ex)
{
    Console.WriteLine($"Invalid email: {ex.Message}");
}
catch (CommandParserBaseException ex)
{
    Console.WriteLine($"Parser error: {ex.Message}");
}
```

## Automatic Help Generation

Help is automatically generated when using standard help flags:

```bash
myProgram.exe --help
myProgram.exe -h
myProgram.exe /?
myProgram.exe -help
```

The help text lists all verbs (if using verbs), parameters, their types, whether they're required, and their descriptions.

## Examples

* [Basic Example](https://github.com/tresvi/CommandLineParser/wiki/Basic-Example)
* [Example with Checkers and Formatters](https://github.com/tresvi/CommandLineParser/wiki/Example-with-Checkers-and-Formatters)

## How It Works

CommandLineParser uses .NET Reflection to:
1. Inspect class properties and their attributes
2. Determine parameter types and validation rules
3. Parse command-line arguments
4. Validate values according to attribute rules
5. Populate the target object instance

This approach eliminates the need for manual parsing and validation code.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

[Specify your license here]

## Links and References

* The Art of UNIX Programming - Eric S. Raymond
* [POSIX Utility Conventions](https://pubs.opengroup.org/onlinepubs/9699919799/basedefs/V1_chap12.html)
* [Linux Standard Options](https://tldp.org/LDP/abs/html/standard-options.html)
* [GNU Command-Line Interface Standards](https://www.gnu.org/prep/standards/html_node/Command_002dLine-Interfaces.html)
* [.NET Core CLI Tools](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run)

---

**Note:** For Spanish documentation, see [README-es.md](README-es.md)

