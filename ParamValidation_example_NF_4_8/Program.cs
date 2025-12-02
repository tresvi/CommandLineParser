using Tresvi.CommandParser;
using Tresvi.CommandParser.Exceptions;
using System;

namespace ParamValidation_Example_NF_4_8
{
    /* 
     * Ejemplos de invocación válidos:
     * 
     * Ejemplo 1: Validación básica con ambiente, email e IP
     * .\ParamValidation_Example_NF_4_8.exe --environment dev --email admin@example.com --server-ip 192.168.1.1 --config-file "C:\Temp\config.txt"
     * 
     * Ejemplo 2: Con código de producto y teléfono (validación con regex)
     * .\ParamValidation_Example_NF_4_8.exe -e prod -m user@domain.com -i 10.0.0.1 -c "C:\Temp\config.txt" -p ABC1234 -t +1-555-123-4567
     * 
     * Ejemplo 3: Con validación de archivos y directorios
     * .\ParamValidation_Example_NF_4_8.exe -e test -m test@example.com -i 172.16.0.1 -c "C:\Temp\existing.txt" -l "C:\Temp\Logs" -w "C:\Temp\NewWork"
     * 
     * Ejemplo 4: Con IPv4 específico para base de datos
     * .\ParamValidation_Example_NF_4_8.exe -e dev -m admin@test.com -i ::1 -d 192.168.1.100 -c "C:\Temp\config.txt"
     * 
     * Ejemplo 5: Con IP y puerto (validación de puerto)
     * .\ParamValidation_Example_NF_4_8.exe -e prod -m admin@example.com -i 192.168.1.1 -c "C:\Temp\config.txt" -a 192.168.1.1:8080
     * 
     * Ejemplo 6: Con IPv6 y puerto
     * .\ParamValidation_Example_NF_4_8.exe -e dev -m admin@example.com -i ::1 -c "C:\Temp\config.txt" -a "[::1]:443"
     * 
     * Ejemplos de invocación que fallarán (para demostrar validaciones):
     * 
     * Falla: Ambiente inválido
     * .\ParamValidation_Example_NF_4_8.exe -e staging -m admin@example.com -i 192.168.1.1 -c "C:\Temp\config.txt"
     * 
     * Falla: Email inválido
     * .\ParamValidation_Example_NF_4_8.exe -e dev -m email-invalido -i 192.168.1.1 -c "C:\Temp\config.txt"
     * 
     * Falla: IP inválida
     * .\ParamValidation_Example_NF_4_8.exe -e dev -m admin@example.com -i 999.999.999.999 -c "C:\Temp\config.txt"
     * 
     * Falla: IP con puerto cuando PortUsage es Never
     * .\ParamValidation_Example_NF_4_8.exe -e dev -m admin@example.com -i 192.168.1.1:8080 -c "C:\Temp\config.txt"
     * 
     * Falla: IP sin puerto cuando PortUsage es Required
     * .\ParamValidation_Example_NF_4_8.exe -e dev -m admin@example.com -i 192.168.1.1 -c "C:\Temp\config.txt" -a 192.168.1.1
     * 
     * Falla: Puerto inválido (fuera de rango)
     * .\ParamValidation_Example_NF_4_8.exe -e dev -m admin@example.com -i 192.168.1.1 -c "C:\Temp\config.txt" -a 192.168.1.1:99999
     * 
     * Falla: Código de producto con formato incorrecto
     * .\ParamValidation_Example_NF_4_8.exe -e dev -m admin@example.com -i 192.168.1.1 -c "C:\Temp\config.txt" -p abc123
     * 
     * Falla: Archivo de configuración no existe
     * .\ParamValidation_Example_NF_4_8.exe -e dev -m admin@example.com -i 192.168.1.1 -c "C:\Temp\noexiste.txt"
     */
	 
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parameters parametros = CommandLine.Parse<Parameters>(args);

                Console.WriteLine("=== Parámetros validados correctamente ===");
                Console.WriteLine($"Ambiente: {parametros.Environment}");
                Console.WriteLine($"Email: {parametros.Email}");
                Console.WriteLine($"IP del Servidor: {parametros.ServerIP}");
                
                if (!string.IsNullOrEmpty(parametros.DatabaseIP))
                    Console.WriteLine($"IP de Base de Datos: {parametros.DatabaseIP}");
                
                if (!string.IsNullOrEmpty(parametros.ProductCode))
                    Console.WriteLine($"Código de Producto: {parametros.ProductCode}");
                
                if (!string.IsNullOrEmpty(parametros.Phone))
                    Console.WriteLine($"Teléfono: {parametros.Phone}");
                
                Console.WriteLine($"Archivo de Configuración: {parametros.ConfigFile}");
                
                if (!string.IsNullOrEmpty(parametros.OutputFile))
                    Console.WriteLine($"Archivo de Salida: {parametros.OutputFile}");
                
                if (!string.IsNullOrEmpty(parametros.LogDirectory))
                    Console.WriteLine($"Directorio de Logs: {parametros.LogDirectory}");
                
                if (!string.IsNullOrEmpty(parametros.WorkDirectory))
                    Console.WriteLine($"Directorio de Trabajo: {parametros.WorkDirectory}");
                
                if (!string.IsNullOrEmpty(parametros.ApiEndpoint))
                    Console.WriteLine($"Endpoint de API: {parametros.ApiEndpoint}");

                Console.WriteLine();
                Console.WriteLine("Todos los parámetros fueron validados exitosamente.");
                Console.WriteLine("Fin OK!!");
            }
            catch (InvalidStringListValueException ex)
            {
                Console.WriteLine($"ERROR DE VALIDACIÓN (Lista de Valores): {ex.Message}");
            }
            catch (InvalidEmailAddressException ex)
            {
                Console.WriteLine($"ERROR DE VALIDACIÓN (Email): {ex.Message}");
            }
            catch (InvalidIPAddressException ex)
            {
                Console.WriteLine($"ERROR DE VALIDACIÓN (IP): {ex.Message}");
            }
            catch (InvalidFormatException ex)
            {
                Console.WriteLine($"ERROR DE VALIDACIÓN (Formato/Regex): {ex.Message}");
            }
            catch (FileNotExistsException ex)
            {
                Console.WriteLine($"ERROR DE VALIDACIÓN (Archivo no existe): {ex.Message}");
            }
            catch (FileAlreadyExistsException ex)
            {
                Console.WriteLine($"ERROR DE VALIDACIÓN (Archivo ya existe): {ex.Message}");
            }
            catch (DirectoryNotExistsException ex)
            {
                Console.WriteLine($"ERROR DE VALIDACIÓN (Directorio no existe): {ex.Message}");
            }
            catch (DirectoryAlreadyExistsException ex)
            {
                Console.WriteLine($"ERROR DE VALIDACIÓN (Directorio ya existe): {ex.Message}");
            }
            catch (CommandParserBaseException ex)
            {
                Console.WriteLine($"ERROR DEL PARSER: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR INESPERADO: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}

