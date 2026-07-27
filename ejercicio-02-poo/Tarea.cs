using System;

public enum Prioridad
{
    Baja,
    Media,
    Alta,
    Critica
}

public class Tarea : IExportable
{
 
    private static int contadorId = 1;

    public int Id { get; private set; }

    public string Titulo { get; set; }

    public string Descripcion { get; set; }

    public Prioridad Prioridad { get; set; }

    public Categoria Categoria { get; set; }

    public bool Completada { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Tarea(
        string titulo,
        string descripcion,
        Prioridad prioridad,
        Categoria categoria)
    {
        Id = contadorId++;

        Titulo = titulo;

        Descripcion = descripcion;

        Prioridad = prioridad;

        Categoria = categoria;

        Completada = false;

        FechaCreacion = DateTime.Now;
    }

    public virtual void MostrarInfo()
    {
        Console.WriteLine("----------------------------------------");

        Console.WriteLine($"ID: {Id}");

        Console.WriteLine($"Título: {Titulo}");

        Console.WriteLine($"Descripción: {Descripcion}");

        Console.WriteLine($"Prioridad: {Prioridad}");

        Console.WriteLine($"Categoría: {Categoria}");

        Console.WriteLine($"Completada: {(Completada ? "Sí" : "No")}");

        Console.WriteLine($"Fecha creación: {FechaCreacion}");
    }

    public string Exportar()
    {
        return $"{Id}|{Titulo}|{Prioridad}|{Completada}";
    }

        public void RestaurarId(int id)
    {
          Id = id;
    }
    public static void ActualizarContador(int ultimoId)
    {
        contadorId = ultimoId + 1;
    }
}

public class TareaConVencimiento : Tarea
{

    public DateTime FechaVencimiento { get; set; }

    public int DiasRestantes
    {
        get
        {
            return (FechaVencimiento.Date - DateTime.Now.Date).Days;
        }
    }

    public TareaConVencimiento(
        string titulo,
        string descripcion,
        Prioridad prioridad,
        Categoria categoria,
        DateTime fechaVencimiento)

        : base(titulo, descripcion, prioridad, categoria)
    {
        FechaVencimiento = fechaVencimiento;
    }

    public override void MostrarInfo()
    {
        base.MostrarInfo();

        Console.WriteLine($"Fecha vencimiento: {FechaVencimiento:d}");

        Console.WriteLine($"Días restantes: {DiasRestantes}");
    }
}