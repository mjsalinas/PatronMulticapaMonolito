
using System.Globalization;
using System.Text.RegularExpressions;

public class TextNormalizer : ITextNormalizer
{
    public string Normalizar(string? titulo)
    {
         if (string.IsNullOrWhiteSpace(titulo))
            return string.Empty;

        var colapsado = Regex.Replace(titulo.Trim(), @"\s+", " ");
        var cultura = CultureInfo.GetCultureInfo("es-HN");
        return cultura.TextInfo.ToTitleCase(colapsado.ToLower(cultura));
    }
}