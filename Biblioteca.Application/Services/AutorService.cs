using Biblioteca.Application.Common;
using Biblioteca.Application.Interfaces.Repositories;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Validation;

namespace Biblioteca.Application.Services;

public class AutorService
{
    private readonly IAutorRepository _autorRepository;
    private readonly ILibroRepository _libroRepository;
    private readonly IAutorValidator _autorValidator;
    private readonly ITextNormalizer _textNormalizer;

    public AutorService(
        IAutorRepository autorRepository,
        ILibroRepository libroRepository,
        IAutorValidator autorValidator,
        ITextNormalizer textNormalizer)
    {
        _autorRepository = autorRepository;
        _libroRepository = libroRepository;
        _autorValidator = autorValidator;
        _textNormalizer = textNormalizer;
    }

    public async Task<(EstadoResultado Estado, string? Mensaje, List<Autor>? Autores)> ObtenerTodosAsync()
    {
        var autores = await _autorRepository.GetAllAsync();
        return (EstadoResultado.Exito, null, autores);
    }

    public async Task<(EstadoResultado Estado, string? Mensaje, Autor? Autor)> ObtenerPorIdAsync(int id)
    {
        var autor = await _autorRepository.GetByIdAsync(id);
        if (autor is null)
            return (EstadoResultado.NoEncontrado, "No se encontró el autor solicitado.", null);

        return (EstadoResultado.Exito, null, autor);
    }

    public async Task<(EstadoResultado Estado, string? Mensaje, Autor? Autor)> CrearAsync(Autor autor)
    {
        autor.Nombre = _textNormalizer.Normalizar(autor.Nombre);
        autor.Nacionalidad = _textNormalizer.Normalizar(autor.Nacionalidad);

        var error = _autorValidator.Validar(autor);
        if (error is not null)
            return (EstadoResultado.Invalido, error, null);

        if (await _autorRepository.ExistsDuplicadoAsync(autor.Nombre))
            return (EstadoResultado.Conflicto, "Ya existe un autor registrado con ese nombre.", null);

        await _autorRepository.AddAsync(autor);
        return (EstadoResultado.Exito, null, autor);
    }

    public async Task<(EstadoResultado Estado, string? Mensaje, Autor? Autor)> ActualizarAsync(int id, Autor autor)
    {
        var existente = await _autorRepository.GetByIdAsync(id);
        if (existente is null)
            return (EstadoResultado.NoEncontrado, "No se encontró el autor a actualizar.", null);

        autor.Nombre = _textNormalizer.Normalizar(autor.Nombre);
        autor.Nacionalidad = _textNormalizer.Normalizar(autor.Nacionalidad);

        var error = _autorValidator.Validar(autor);
        if (error is not null)
            return (EstadoResultado.Invalido, error, null);

        if (await _autorRepository.ExistsDuplicadoAsync(autor.Nombre, id))
            return (EstadoResultado.Conflicto, "Ya existe otro autor registrado con ese nombre.", null);

        existente.Nombre = autor.Nombre;
        existente.Nacionalidad = autor.Nacionalidad;

        await _autorRepository.UpdateAsync(existente);
        return (EstadoResultado.Exito, null, existente);
    }

    public async Task<(EstadoResultado Estado, string? Mensaje)> EliminarAsync(int id)
    {
        var existente = await _autorRepository.GetByIdAsync(id);
        if (existente is null)
            return (EstadoResultado.NoEncontrado, "No se encontró el autor a eliminar.");

        if (await _libroRepository.ExistsByAutorIdAsync(id))
            return (EstadoResultado.Conflicto, "No se puede eliminar un autor que tiene libros registrados.");

        await _autorRepository.DeleteAsync(existente);
        return (EstadoResultado.Exito, null);
    }
}
