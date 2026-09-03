using System.Text.RegularExpressions;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Validation;

public class LibroValidator : ILibroValidator
{
    private const int AnioMinimoPublicacion = 1440;
    private const int TituloMinLength = 3;
    private const int TituloMaxLength = 200;
    public string? Validar(Libro libro)
    {
        if (libro.Titulo.Length < TituloMinLength)
            return $"El título debe tener al menos {TituloMinLength} caracteres.";

        if (libro.Titulo.Length > TituloMaxLength)
            return $"El título no puede superar los {TituloMaxLength} caracteres.";

        if (!Regex.IsMatch(libro.Titulo, @"[\p{L}]"))
            return "El título debe contener al menos una letra.";

        if (libro.AutorId <= 0)
            return "Debe indicar un autor válido.";

        if (libro.AnioPublicacion < AnioMinimoPublicacion)
            return $"El año de publicación no puede ser anterior a {AnioMinimoPublicacion}.";

        if (libro.AnioPublicacion > DateTime.Now.Year)
            return "El año de publicación no puede ser futuro.";

        return null;
    }
}