using System;

class Program
{

    static GestorTareas gestor = new GestorTareas();

    static void Main(string[] args)
    {
        gestor.CargarDeJSON("tareas.json");

        int opcion;

        do
        {
            MostrarMenu();

            Console.Write("Seleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("\nOpción inválida.\n");
                continue;
            }

            Console.WriteLine();

            switch (opcion)
            {
               case 1:

        AgregarTarea();

        break;

    case 2:

        ListarTodas();

        break;

    case 3:

        ListarPorCategoria();

        break;

    case 4:

        ListarPorPrioridad();

        break;

    case 5:

        CompletarTarea();

        break;

    case 6:

        MostrarVencidas();

        break;

    case 7:

        EliminarTarea();

        break;

    case 8:

        ExportarJSON();

        break;

                case 9:

                    gestor.GuardarEnJSON("tareas.json");

                    Console.WriteLine();

                    Console.WriteLine("Las tareas fueron guardadas correctamente.");
                    
                    Console.WriteLine("Hasta luego.");

                    break;

                default:

                    Console.WriteLine("Opción no válida.");

                    break;
            }

            if (opcion != 9)
            {
                Console.WriteLine();

                Console.WriteLine("Presione una tecla para continuar...");

                Console.ReadKey();

                Console.Clear();
            }

        } while (opcion != 9);
    }

    static void MostrarMenu()
    {
        Console.WriteLine("======================================");
        Console.WriteLine("     ===== GESTOR DE TAREAS ====");
        Console.WriteLine("======================================");
        Console.WriteLine("1. Agregar tarea");
        Console.WriteLine("2. Listar todas");
        Console.WriteLine("3. Listar por categoría");
        Console.WriteLine("4. Listar por prioridad");
        Console.WriteLine("5. Marcar como completada");
        Console.WriteLine("6. Mostrar tareas vencidas");
        Console.WriteLine("7. Eliminar tarea");
        Console.WriteLine("8. Exportar a JSON");
        Console.WriteLine("9. Salir");
        Console.WriteLine("======================================");
    }

    static void AgregarTarea()
    {
        Console.WriteLine("=== AGREGAR TAREA ===");

        Console.Write("Título: ");
        string titulo = Console.ReadLine()!;

        Console.Write("Descripción: ");
        string descripcion = Console.ReadLine()!;

        Console.WriteLine();

        Console.WriteLine("Prioridades");

        Console.WriteLine("1. Baja");
        Console.WriteLine("2. Media");
        Console.WriteLine("3. Alta");
        Console.WriteLine("4. Crítica");

        Console.Write("Seleccione una prioridad: ");

        int opcionPrioridad;

        while (!int.TryParse(Console.ReadLine(), out opcionPrioridad) || opcionPrioridad < 1 || opcionPrioridad > 4)
        {
            Console.Write("Opción inválida. Intente nuevamente: ");
        }

        Prioridad prioridad = (Prioridad)(opcionPrioridad - 1);

        Console.Write("Categoría: ");

        string nombreCategoria = Console.ReadLine()!;

        Categoria categoria = new Categoria(nombreCategoria,"",""); 
        
        Console.WriteLine();

        Console.Write("¿La tarea tiene fecha de vencimiento? (S/N): ");

        string respuesta = Console.ReadLine()!.Trim().ToUpper();

        if (respuesta == "S")
        {
            DateTime fecha;

            Console.Write("Fecha de vencimiento (dd/MM/yyyy): ");

            while (!DateTime.TryParse(Console.ReadLine(), out fecha))
            {
                Console.Write("Fecha inválida. Intente nuevamente: ");
            }

            Tarea tarea = new TareaConVencimiento(
                titulo,
                descripcion,
                prioridad,
                categoria,
                fecha);

            gestor.Agregar(tarea);
        }
        else
        {
            Tarea tarea = new Tarea(
                titulo,
                descripcion,
                prioridad,
                categoria);

            gestor.Agregar(tarea);
        }

        Console.WriteLine();

        Console.WriteLine("Tarea agregada correctamente.");
    }
    static void ListarTodas()
{
    Console.WriteLine("=== TODAS LAS TAREAS ===");

    if (gestor.Tareas.Count == 0)
    {
        Console.WriteLine("No hay tareas registradas.");
        return;
    }

    foreach (Tarea tarea in gestor.Tareas)
    {
        tarea.MostrarInfo();

        Console.WriteLine();
    }
}

static void ListarPorCategoria()
{
    Console.Write("Ingrese la categoría: ");

    string categoria = Console.ReadLine()!;

    List<Tarea> lista = gestor.ListarPorCategoria(categoria);

    if (lista.Count == 0)
    {
        Console.WriteLine("No se encontraron tareas.");
        return;
    }

    foreach (Tarea tarea in lista)
    {
        tarea.MostrarInfo();

        Console.WriteLine();
    }
}

static void ListarPorPrioridad()
{
    Console.WriteLine("1. Baja");
    Console.WriteLine("2. Media");
    Console.WriteLine("3. Alta");
    Console.WriteLine("4. Crítica");

    Console.Write("Seleccione una prioridad: ");

    int opcion;

    while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 4)
    {
        Console.Write("Opción inválida: ");
    }

    Prioridad prioridad = (Prioridad)(opcion - 1);

    List<Tarea> lista = gestor.ListarPorPrioridad(prioridad);

    if (lista.Count == 0)
    {
        Console.WriteLine("No existen tareas.");
        return;
    }

    foreach (Tarea tarea in lista)
    {
        tarea.MostrarInfo();

        Console.WriteLine();
    }
}

static void CompletarTarea()
{
    Console.Write("Ingrese el ID: ");

    int id;

    if (int.TryParse(Console.ReadLine(), out id))
    {
        gestor.Completar(id);

        Console.WriteLine("Proceso finalizado.");
    }
}

static void MostrarVencidas()
{
    List<Tarea> lista = gestor.ObtenerVencidas();

    if (lista.Count == 0)
    {
        Console.WriteLine("No existen tareas vencidas.");

        return;
    }

    foreach (Tarea tarea in lista)
    {
        tarea.MostrarInfo();

        Console.WriteLine();
    }
}

static void EliminarTarea()
{
    Console.Write("Ingrese el ID: ");

    int id;

    if (int.TryParse(Console.ReadLine(), out id))
    {
        gestor.Eliminar(id);

        Console.WriteLine("Proceso finalizado.");
    }
}
static void ExportarJSON()
{
     try
    {
        gestor.GuardarEnJSON("tareas.json");

        Console.WriteLine();

        Console.WriteLine("Las tareas fueron exportadas correctamente.");

        Console.WriteLine("Archivo generado: tareas.json");
    }
    catch (Exception ex)
    {
        Console.WriteLine();

        Console.WriteLine("Ocurrió un error al exportar el archivo JSON.");

        Console.WriteLine(ex.Message);
    }
}

}

