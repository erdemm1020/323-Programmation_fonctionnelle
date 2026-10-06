using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace FpAide;

/// <summary>Affichage console. Show(expr) affiche le CODE de l'expression + son RÉSULTAT.</summary>
public static class Out
{
    public static void Title(string t)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n══════ {t} ══════");
        Console.ResetColor();
    }

    public static void Section(string t)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n── {t}");
        Console.ResetColor();
    }

    public static void Note(string t)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"   ℹ {t}");
        Console.ResetColor();
    }

    public static void Warn(string t)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"   ⚠ {t}");
        Console.ResetColor();
    }

    /// <summary>Affiche "expression  →  résultat" (le texte de l'expression vient du code source).</summary>
    public static void Show<T>(T value, [CallerArgumentExpression("value")] string expr = "")
    {
        expr = Regex.Replace(expr, @"\s+", " ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"   {expr}");
        Console.ResetColor();
        Console.WriteLine($"  →  {Fmt(value)}");
    }

    /// <summary>Exécute f et affiche soit le résultat, soit l'exception levée.</summary>
    public static void Try<T>(Func<T> f, [CallerArgumentExpression("f")] string expr = "")
    {
        try { Show(f(), expr); }
        catch (Exception e)
        {
            expr = Regex.Replace(expr, @"\s+", " ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"   {expr}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  →  💥 {e.GetType().Name}");
            Console.ResetColor();
        }
    }

    public static string Fmt(object? v) => v switch
    {
        null => "null",
        string s => $"\"{s}\"",
        double d => d.ToString("0.##", CultureInfo.InvariantCulture),
        IEnumerable e => "[" + string.Join(", ", e.Cast<object?>().Select(Fmt)) + "]",
        _ => v.ToString() ?? ""
    };
}
