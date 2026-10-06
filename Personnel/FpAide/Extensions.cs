namespace FpAide;

// Thématique 05 : classe PUBLIC STATIC + méthode PUBLIC STATIC + "this" sur le 1er paramètre
public static class StringExtensions
{
    public static string Greetings(this string name) => $"Hello {name}";
    public static string RemoveSpaces(this string s) => s.Replace(" ", "");
    public static string ToConsole(this string s) { Console.WriteLine("   " + s); return s; }
}

public static class EnumerableExtensions
{
    // Sépare les éléments par un séparateur
    public static string JoinWith(this IEnumerable<string> source, string sep) => string.Join(sep, source);

    // Ma propre version de Where (HOF + évaluation PARESSEUSE grâce à yield)
    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var item in source)
            if (predicate(item)) yield return item;
    }

    // Ma propre version de Select
    public static IEnumerable<TOut> MySelect<T, TOut>(this IEnumerable<T> source, Func<T, TOut> map)
    {
        foreach (var item in source) yield return map(item);
    }

    // Mon propre Aggregate (Fold gauche) — itératif, donc pas de stack overflow
    public static TAcc MyFold<T, TAcc>(this IEnumerable<T> source, TAcc seed, Func<TAcc, T, TAcc> f)
    {
        var acc = seed;
        foreach (var item in source) acc = f(acc, item);
        return acc;
    }

    // Exemple du cours : retourne IEnumerable<string> pour permettre le chaînage
    public static IEnumerable<string> ToLower(this IEnumerable<string> source, bool random = false)
        => source.Select(text => random ? (new Random().Next(2) == 1 ? text.ToLower() : text) : text.ToLower());
}

public static class FuncExtensions
{
    // Composition : (f.Then(g))(x) = g(f(x))   — attention à l'ordre !
    public static Func<A, C> Then<A, B, C>(this Func<A, B> f, Func<B, C> g) => x => g(f(x));

    // Pipe : x.Pipe(f) = f(x)
    public static R Pipe<T, R>(this T x, Func<T, R> f) => f(x);
}
