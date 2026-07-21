using System;
using System.IO;
class Program
{
    // Variables para las estadísticas correspondientes
    static int totalProcesadas = 0;
    static int totalValidas = 0;
    static int totalInvalidas = 0;

    static int totalVisa = 0;
    static int totalMastercard = 0;
    static int totalAmex = 0;
    static int totalDiscover = 0;
    static int totalDesconocidas = 0;

    // Método principal del programa / Controla el menú y permite acceder a cada una de las funcionalidades.
    static void Main(string[] args)
    {
        int opcion;

        do
        {
            MostrarMenu();

            Console.Write("\nSeleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("\nOpción inválida.");
                continue;
            }

            switch (opcion)
            {
                case 1:
                    
                    Console.Write("\nIngrese el número de la tarjeta: ");

                    string numero = Console.ReadLine()!;

                    string marca = IdentificarMarca(numero);

                    bool valida = ValidarTarjeta(numero);
                    
                    Console.WriteLine();

                    Console.WriteLine($"Número: {numero}");
                    Console.WriteLine($"Marca: {marca}");

                    if (valida)
                    Console.WriteLine("Estado:✅ VÁLIDA");
                    else
                    Console.WriteLine("Estado:❌ INVÁLIDA");

                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();

                    break;

                case 2:

                    ValidarDesdeArchivo("tarjetas.txt");

                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();

                    break;

                case 3:

                    string tarjetaGenerada = GenerarNumeroValido();

                    Console.WriteLine();
                    Console.WriteLine("========= TARJETA GENERADA =========");
                    Console.WriteLine($"Número : {tarjetaGenerada}");
                    Console.WriteLine($"Marca  : {IdentificarMarca(tarjetaGenerada)}");
                    Console.WriteLine("Estado : ✅ VÁLIDA");

                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();

                    break;

                case 4:

                    MostrarEstadisticas();

                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();

                    break;

                case 5:

                    Console.WriteLine("\nPrograma finalizado.");

                    break;

                default:

                    Console.WriteLine("\nOpción incorrecta.");

                    break;
            }

        } while (opcion != 5);

    }
    static void MostrarMenu()
    {
        Console.WriteLine();
        Console.WriteLine("==================================");
        Console.WriteLine("=== VALIDADOR DE TARJETAS ===");
        Console.WriteLine("==================================");
        Console.WriteLine("1. Validar una tarjeta");
        Console.WriteLine("2. Validar desde archivo");
        Console.WriteLine("3. Generar número válido");
        Console.WriteLine("4. Estadísticas");
        Console.WriteLine("5. Salir");
    }

    // Implementa el algoritmo de Luhn para determinar si un número de tarjeta es válido.
    static bool ValidarTarjeta(string numero)
    
    {
        if (string.IsNullOrWhiteSpace(numero))
        return false;

    // Verificar que solo tenga números
    foreach (char c in numero)
    {
        
        if (!char.IsDigit(c))
            return false;
    }

    string marca = IdentificarMarca(numero);

if (marca == "Desconocida")
    return false;

    int suma = 0;

    bool duplicar = false;

    // Se recorre el número de derecha a izquierda, Cada dígito se duplica según el algoritmo de Luhn.
    for (int i = numero.Length - 1; i >= 0; i--)
    {
        int digito = numero[i] - '0';

        if (duplicar)
        {
            digito *= 2;

            if (digito >= 10)
            {
                digito = (digito / 10) + (digito % 10);
            }
        }

        suma += digito;

        duplicar = !duplicar;
    }

    return suma % 10 == 0;
}
    
    // Identifica la marca de la tarjeta según el prefijo y la longitud del número.
    static string IdentificarMarca(string numero)
{
    // Si viene vacío
    if (string.IsNullOrWhiteSpace(numero))
        return "Desconocida";

    // Contiene letras o símbolos
    foreach (char c in numero)
    {
        if (!char.IsDigit(c))
            return "Desconocida";
    }
    
    // ======== Tarjetas ========//

    if (numero.StartsWith("4"))
    {
        return "Visa";
    }

        if (numero.StartsWith("51") ||
            numero.StartsWith("52") ||
            numero.StartsWith("53") ||
            numero.StartsWith("54") ||
            numero.StartsWith("55"))
        {
            return "Mastercard";
        }
    
        if (numero.StartsWith("34") ||
            numero.StartsWith("37"))
        {
            return "American Express";
        }

   
        if (numero.StartsWith("6011") || numero.StartsWith("65"))
        {
            return "Discover";
        }

        if (numero.Length >= 3)
        {
            int prefijo3 = Convert.ToInt32(numero.Substring(0, 3));

            if (prefijo3 >= 644 && prefijo3 <= 649)
            {
                return "Discover";
            }
        }

        if (numero.Length >= 6)
        {
            int prefijo6 = Convert.ToInt32(numero.Substring(0, 6));

            if (prefijo6 >= 622126 && prefijo6 <= 622925)
            {
                return "Discover";
            }
        }
    
    return "Desconocida";
}

    // Actualiza los contadores generales de tarjetas procesadas, válidas, inválidas y por marca.
    static void ActualizarEstadisticas(bool valida, string marca)
{
    totalProcesadas++;

    if (valida)
        totalValidas++;
    else
        totalInvalidas++;

    switch (marca)
    {
        case "Visa":
            totalVisa++;
            break;

        case "Mastercard":
            totalMastercard++;
            break;

        case "American Express":
            totalAmex++;
            break;

        case "Discover":
            totalDiscover++;
            break;

        default:
            totalDesconocidas++;
            break;
    }
}
    // Muestra el resumen de todas las tarjetas procesadas durante la ejecución del programa.
    static void MostrarEstadisticas()
{
    Console.WriteLine();
    Console.WriteLine("======== ESTADÍSTICAS ========");
    Console.WriteLine();

    Console.WriteLine($"Total procesadas     : {totalProcesadas}");
    Console.WriteLine($"Tarjetas válidas     : {totalValidas}");
    Console.WriteLine($"Tarjetas inválidas   : {totalInvalidas}");

    Console.WriteLine();

    Console.WriteLine($"Visa                 : {totalVisa}");
    Console.WriteLine($"Mastercard           : {totalMastercard}");
    Console.WriteLine($"American Express     : {totalAmex}");
    Console.WriteLine($"Discover             : {totalDiscover}");
    Console.WriteLine($"Desconocidas         : {totalDesconocidas}");
}

    // Lee un archivo de texto con un número de tarjeta por línea y procesa cada registro.
   static void ValidarDesdeArchivo(string ruta)
{
    try
    {   
        totalProcesadas = 0;
        totalValidas = 0;
        totalInvalidas = 0;

        totalVisa = 0;
        totalMastercard = 0;
        totalAmex = 0;
        totalDiscover = 0;
        totalDesconocidas = 0;

        if (!File.Exists(ruta))
        {
            Console.WriteLine("\nNo se encontró el archivo tarjetas.txt");
            return;
        }

        string[] tarjetas = File.ReadAllLines(ruta);

        Console.WriteLine();
        Console.WriteLine("========= RESULTADOS DE LA VALIDACION =========");
        Console.WriteLine();

        // Recorrer todas las tarjetas almacenadas en el archivo.
        foreach (string tarjeta in tarjetas)
        {
            string numero = tarjeta.Trim();

            if (numero == "")
                continue;

            string marca = IdentificarMarca(numero);

            bool valida = ValidarTarjeta(numero);

            ActualizarEstadisticas(valida, marca);
        
            Console.WriteLine($"Número : {numero}");
            Console.WriteLine($"Marca  : {marca}");

            if (valida)
            {
                Console.WriteLine("Estado :✅ VÁLIDA");
            }
            else
            {
                Console.WriteLine("Estado :❌ INVÁLIDA");
            }

            Console.WriteLine("--------------------------------");
        }

            Console.WriteLine();
            Console.WriteLine("========== RESUMEN ==========");
            Console.WriteLine();

            Console.WriteLine($"Total procesadas     : {totalProcesadas}");
            Console.WriteLine($"Tarjetas válidas     : {totalValidas}");
            Console.WriteLine($"Tarjetas inválidas   : {totalInvalidas}");

            Console.WriteLine();

            Console.WriteLine($"Visa                 : {totalVisa}");
            Console.WriteLine($"Mastercard           : {totalMastercard}");
            Console.WriteLine($"American Express     : {totalAmex}");
            Console.WriteLine($"Discover             : {totalDiscover}");
            Console.WriteLine($"Desconocidas         : {totalDesconocidas}");

    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al leer el archivo.");
        Console.WriteLine(ex.Message);
    }
}

    // Genera un número de tarjeta válido calculando el dígito verificador mediante Luhn.
    static string GenerarNumeroValido()
    {
       Random random = new Random();

    string numero = "";

    // Elegir una marca aleatoriamente
    int opcion = random.Next(1, 5);

    switch (opcion)
    {
        case 1:
            numero = "4";     
            break;

        case 2:
            numero = "51";  
            break;

        case 3:
            numero = "34";  
            break;

        case 4:
            numero = "6011"; 
            break;
    }

    int longitud;

    if (numero.StartsWith("34"))
        longitud = 15;
    else
        longitud = 16;

    // Completar con números aleatorios dejando libre el último dígito
    while (numero.Length < longitud - 1)
    {
        numero += random.Next(0, 10);
    }

    for (int digito = 0; digito <= 9; digito++)
    {
        string candidato = numero + digito;

        if (ValidarTarjeta(candidato))
        {
            return candidato;
        }
    }
    return "";
    }

}
