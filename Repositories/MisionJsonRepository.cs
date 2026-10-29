using DSW_3P_PR01.Models;
using System.Text.Json;

namespace DSW_3P_PR01.Repositories;

public class MisionJsonRepository : IMisionRepository
{
    private readonly string _ruta;
    private readonly JsonSerializerOptions _opciones = new()
    {
        WriteIndented = true
    };

    public MisionJsonRepository(IWebHostEnvironment environment)
    {
        _ruta = Path.Combine(environment.ContentRootPath, "Data", "misiones.json");

        Directory.CreateDirectory(Path.GetDirectoryName(_ruta)!);

        if (!File.Exists(_ruta))
        {
            File.WriteAllText(_ruta, "[]");
        }
    }

    private List<Mision> Leer()
    {
        string Json = File.ReadAllText(_ruta);

        return JsonSerializer.Deserialize<List<Mision>>(Json) ?? new List<Mision>();
    }

    private void Guardar(List<Mision> datos)
    {
        string Json = JsonSerializer.Serialize(datos, _opciones);
        
        File.WriteAllText(_ruta, Json);
    }

    public List<Mision> ObtenerTodas()
    {
        return Leer();
    }

    public Mision? ObtenerPorId(int id)
    {
        return Leer().FirstOrDefault(d => d.Id == id);
    }

    public Mision Agregar(Mision nueva)
    {
        var datos = Leer();

        nueva.Id = datos.Count == 0 ? 1 : datos.Max(d => d.Id + 1);
        datos.Add(nueva);
        Guardar(datos);

        return nueva;
    }

    public bool Actualizar(int id, Mision actualizada)
    {
        var datos = Leer();

        var existente = datos.FirstOrDefault(d => d.Id == id);

        if(existente == null)
        {
            return false;
        }
        existente.Nombre = actualizada.Nombre;
        existente.Dificultad = actualizada.Dificultad;
        existente.RecompensaXP = actualizada.RecompensaXP;
        existente.Completada = actualizada.Completada;
        
        Guardar(datos);

        return true;
    }

    public bool Eliminar(int id)
    {
        var datos = Leer();

        var existente = datos.FirstOrDefault(d => d.Id == id);

        if(existente == null)
        {
            return false;
        }
        datos.Remove(existente);
        Guardar(datos);

        return true;
    }
}