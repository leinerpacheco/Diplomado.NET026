using System;

public class TareaJson
{
    public string? Tipo {get; set;}

    public int Id {get; set;}

    public string? Titulo {get; set;}

    public string? Descripcion {get; set;}

    public Prioridad Prioridad {get; set;}

    public string? NombreCategoria {get; set;}

    public bool Completada {get; set;}

    public DateTime FechaCreacion {get; set;}

    public DateTime? FechaVencimiento {get; set;}
}