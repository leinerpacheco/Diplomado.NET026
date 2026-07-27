using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

public class GestorTareas
{
    private List<Tarea> tareas;

    public GestorTareas()
    {
        tareas = new List<Tarea>();
    }

    public List<Tarea> Tareas
    {
        get { return tareas; }
    }

    public void Agregar(Tarea tarea)
    {
        tareas.Add(tarea);
    }

    public void Completar(int id)
    {
        Tarea tarea = tareas.FirstOrDefault(t => t.Id == id)!;

        if (tarea != null)
        {
            tarea.Completada = true;
        }
    }

    public List<Tarea> ListarPorCategoria(string categoria)
    {
        return tareas.Where(t => t.Categoria != null && t.Categoria.Nombre.Equals(categoria, 
        StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Tarea> ListarPorPrioridad(Prioridad prioridad)
    {
        return tareas.Where(t => t.Prioridad == prioridad).ToList();
    }

    public List<Tarea> ObtenerVencidas()
    {
        return tareas.Where(t =>t is TareaConVencimiento && ((TareaConVencimiento)t).FechaVencimiento.Date 
        < DateTime.Now.Date && !t.Completada).ToList();
    }

    public void Eliminar(int id)
    {
        Tarea tarea = tareas.FirstOrDefault(t => t.Id == id)!;

        if (tarea != null)
        {
            tareas.Remove(tarea);
        }
    }

    public void GuardarEnJSON(string archivo)
    {
            List<TareaJson> datos = new List<TareaJson>();

    foreach (Tarea tarea in tareas)
    {
        TareaJson registro = new TareaJson();

        registro.Id = tarea.Id;

        registro.Titulo = tarea.Titulo;

        registro.Descripcion = tarea.Descripcion;

        registro.Prioridad = tarea.Prioridad;

        registro.NombreCategoria = tarea.Categoria.Nombre;

        registro.Completada = tarea.Completada;

        registro.FechaCreacion = tarea.FechaCreacion;

        if (tarea is TareaConVencimiento tareaVencimiento)
        {
            registro.Tipo = "TareaConVencimiento";

            registro.FechaVencimiento = tareaVencimiento.FechaVencimiento;
        }
        else
        {
            registro.Tipo = "Tarea";

            registro.FechaVencimiento = null;
        }

        datos.Add(registro);
    }

    JsonSerializerOptions opciones = new JsonSerializerOptions();

    opciones.WriteIndented = true;

    string json = JsonSerializer.Serialize(datos, opciones);

    File.WriteAllText(archivo, json);

    }

    public List<Tarea> CargarDeJSON(string archivo)
    {
        try
    {
        if (!File.Exists(archivo))
        {
            return tareas;
        }

        string json = File.ReadAllText(archivo);

        List<TareaJson>? datos = JsonSerializer.Deserialize<List<TareaJson>>(json);

        if (datos == null)
        {
            return tareas;
        }

        tareas.Clear();

        int ultimoId = 0;

        foreach (TareaJson registro in datos)
        {
            Tarea tarea;

            if (registro.Tipo == "TareaConVencimiento")
            {
                tarea = new TareaConVencimiento(
                    registro.Titulo!,
                    registro.Descripcion!,
                    registro.Prioridad,
                    new Categoria(
                    registro.NombreCategoria!,"",""),
                    registro.FechaVencimiento ?? DateTime.Now);
            }
            else
            {
                tarea = new Tarea(
                    registro.Titulo!,
                    registro.Descripcion!,
                    registro.Prioridad,
                    new Categoria(registro.NombreCategoria!,"",""));
            }

            tarea.RestaurarId(registro.Id);

            tarea.Completada = registro.Completada;

            tarea.FechaCreacion = registro.FechaCreacion;

            tareas.Add(tarea);

            if (registro.Id > ultimoId)
            {
                ultimoId = registro.Id;
            }
        }

        Tarea.ActualizarContador(ultimoId);
    }
    catch
    {
        Console.WriteLine("No fue posible cargar el archivo JSON.");
    }
        return tareas;
    }
}