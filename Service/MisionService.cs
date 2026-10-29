using DSW_3P_PR01.Models;
using DSW_3P_PR01.Repositories;

namespace DSW_3P_PR01.Services;

public class MisionService : IMisionService
{
    private readonly IMisionRepository _repository;

    public MisionService(IMisionRepository repository)
    {
        _repository = repository;
    }

    private static void Validar(Mision mision)
    {
        if (string.IsNullOrWhiteSpace(mision.Nombre))
            throw new ArgumentException("El nombre no puede estar vacio");
        if (mision.Dificultad != "Facil" && mision.Dificultad != "Media" && mision.Dificultad != "Dificil")
            throw new ArgumentException("La dificultad debe ser Facil, Media o Dificil");
        if (mision.RecompensaXP < 0)
            throw new ArgumentException("La experiencia no puede ser negativa");
    }

    public Mision? ObtenerPorId(int id)
    {
        var mision = _repository.ObtenerPorId(id);

        return mision;
    }

    public List<Mision> ObtenerTodas()
    {
        return _repository.ObtenerTodas();
    }

    public Mision Agregar(Mision nuevaMision)
    {
        Validar(nuevaMision);

        return _repository.Agregar(nuevaMision);
    }

    public bool Actualizar(int id, Mision mision)
    {
        Validar(mision);

        return _repository.Actualizar(id, mision);
    }

    public bool Eliminar(int id)
    {
        return _repository.Eliminar(id);
    }
}