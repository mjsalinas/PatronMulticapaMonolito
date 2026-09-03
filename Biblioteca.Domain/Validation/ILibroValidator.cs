using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Validation;

public interface ILibroValidator
{
    string? Validar(Libro libro);
}