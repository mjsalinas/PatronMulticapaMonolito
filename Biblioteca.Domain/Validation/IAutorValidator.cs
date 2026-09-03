using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Validation;

public interface IAutorValidator
{
    string? Validar(Autor autor);
}