using System.Text.RegularExpressions;
using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Validation;

public class AutorValidator : IAutorValidator
{
    private const int NombreMinLength = 2;
    private const int NombreMaxLength = 100;
    private const int NacionalidadMinLength = 3;
    private const int NacionalidadMaxLength = 60;

    private static readonly Regex NombreValidoRegex =
        new(@"^[\p{L}\s'\-\.]+$", RegexOptions.Compiled);
    public string? Validar(Autor autor)
    {
        if (autor.Nombre.Length < NombreMinLength)
            return $"El nombre debe tener al menos {NombreMinLength} caracteres.";

        if (autor.Nombre.Length > NombreMaxLength)
            return $"El nombre no puede superar los {NombreMaxLength} caracteres.";

        if (!NombreValidoRegex.IsMatch(autor.Nombre))
            return "El nombre solo puede contener letras, espacios, guiones o apóstrofos.";

        if (string.IsNullOrWhiteSpace(autor.Nacionalidad))
            return "La nacionalidad del autor es obligatoria.";

        if (autor.Nacionalidad.Length < NacionalidadMinLength)
            return $"La nacionalidad debe tener al menos {NacionalidadMinLength} caracteres.";

        if (autor.Nacionalidad.Length > NacionalidadMaxLength)
            return $"La nacionalidad no puede superar los {NacionalidadMaxLength} caracteres.";

        if (!NombreValidoRegex.IsMatch(autor.Nacionalidad))
            return "La nacionalidad solo puede contener letras, espacios, guiones o apóstrofos.";

        return null;
    }
}