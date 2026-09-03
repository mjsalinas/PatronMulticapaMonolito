using Biblioteca.Application.Common;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Validation;

namespace Biblioteca.Application.Services;

public class LibroService
{
    private const int AnioLimiteEliminacion = 1900;

    private readonly ILibroRepository _libroRepository;
    private readonly IAutorRepository _autorRepository;
    private readonly ILibroValidator _libroValidator;
    private readonly ITextNormalizer _textNormalizer;

    public LibroService(
        ILibroRepository libroRepository,
        IAutorRepository autorRepository,
        ILibroValidator libroValidator,
        ITextNormalizer textNormalizer)
    {
        _libroRepository = libroRepository;
        _autorRepository = autorRepository;
        _libroValidator = libroValidator;
        _textNormalizer = textNormalizer;
    }

    public async Task<(EstadoResultado Estado, string? Mensaje, List<Libro>? Libros)> ObtenerTodosAsync()
    {
        var libros = await _libroRepository.GetAllAsync();
        return (EstadoResultado.Exito, null, libros);
    }

    public async Task<(EstadoResultado Estado, string? Mensaje, Libro? Libro)> ObtenerPorIdAsync(int id)
    {
        var libro = await _libroRepository.GetByIdAsync(id);
        if (libro is null)
            return (EstadoResultado.NoEncontrado, "No se encontró el libro solicitado.", null);

        return (EstadoResultado.Exito, null, libro);
    }

    public async Task<(EstadoResultado Estado, string? Mensaje, Libro? Libro)> CrearAsync(Libro libro)
    {
        libro.Titulo = _textNormalizer.Normalizar(libro.Titulo);

        var error = _libroValidator.Validar(libro);
        if (error is not null)
            return (EstadoResultado.Invalido, error, null);

        if (!await _autorRepository.ExistsAsync(libro.AutorId))
            return (EstadoResultado.Invalido, "El autor indicado no existe.", null);

        await _libroRepository.AddAsync(libro);
        return (EstadoResultado.Exito, null, libro);
    }

    public async Task<(EstadoResultado Estado, string? Mensaje, Libro? Libro)> ActualizarAsync(int id, Libro libro)
    {
        var existente = await _libroRepository.GetByIdAsync(id);
        if (existente is null)
            return (EstadoResultado.NoEncontrado, "No se encontró el libro a actualizar.", null);

        libro.Titulo = _textNormalizer.Normalizar(libro.Titulo);

        var error = _libroValidator.Validar(libro);
        if (error is not null)
            return (EstadoResultado.Invalido, error, null);

        if (!await _autorRepository.ExistsAsync(libro.AutorId))
            return (EstadoResultado.Invalido, "El autor indicado no existe.", null);

        existente.Titulo = libro.Titulo;
        existente.AnioPublicacion = libro.AnioPublicacion;
        existente.AutorId = libro.AutorId;

        await _libroRepository.UpdateAsync(existente);
        return (EstadoResultado.Exito, null, existente);
    }

    public async Task<(EstadoResultado Estado, string? Mensaje)> EliminarAsync(int id)
    {
        var existente = await _libroRepository.GetByIdAsync(id);
        if (existente is null)
            return (EstadoResultado.NoEncontrado, "No se encontró el libro a eliminar.");

        if (existente.AnioPublicacion < AnioLimiteEliminacion)
            return (EstadoResultado.Conflicto, $"No se puede eliminar un libro publicado antes de {AnioLimiteEliminacion} por su valor histórico.");

        await _libroRepository.DeleteAsync(existente);
        return (EstadoResultado.Exito, null);
    }
}
