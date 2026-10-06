namespace FpAide;

// ===== DataSeries<T> : le fil rouge (généricité de TYPE) =====
public class DataSeries<T>
{
    private readonly IEnumerable<T> _data;
    private DataSeries(IEnumerable<T> data) => _data = data;

    public static DataSeries<T> From(IEnumerable<T> source) => new DataSeries<T>(source);

    public int Count => _data.Count();
    public IEnumerable<T> Values => _data;

    // Extensions naturelles : on retourne une NOUVELLE série (immutabilité)
    public DataSeries<T> Where(Func<T, bool> predicate) => new(_data.Where(predicate));
    public DataSeries<TOut> Select<TOut>(Func<T, TOut> map) => DataSeries<TOut>.From(_data.Select(map));
    public TAcc Fold<TAcc>(TAcc seed, Func<TAcc, T, TAcc> f) => _data.Aggregate(seed, f);
}

public static class M01Paradigmes
{
    public static void Run()
    {
        Out.Title("01 · Paradigmes & Généricité");
        var matches = Data.Matches();

        Out.Section("Impératif (COMMENT) vs Déclaratif (QUOI)");
        var winsImp = new List<Match>();                 // impératif : on décrit chaque étape
        foreach (var m in matches)
            if (m.Won) winsImp.Add(m);
        Out.Show(winsImp.Select(m => m.Player));

        Out.Show(matches.Where(m => m.Won).Select(m => m.Player));   // déclaratif : on décrit ce qu'on veut

        Out.Section("Même problème, 3 styles");
        Console.Write("   procédural : ");
        for (int i = 0; i < 5; i++) Console.Write($"Match {i}; ");
        Console.WriteLine();
        Console.Write("   fonctionnel: ");
        Enumerable.Range(0, 5).Select(i => $"Match {i}").ToList().ForEach(s => Console.Write(s + "; "));
        Console.WriteLine();

        Out.Section("Généricité de TYPE : T = paramètre de type");
        var names = new List<string> { "a", "b" };     // T = string
        var scores = new List<int> { 1, 2, 3 };        // T = int
        Out.Show(names.Count + scores.Count);

        var val = DataSeries<Match>.From(matches);
        var temps = DataSeries<double>.From(new[] { 21.5, 22.0, 19.8 });
        Out.Show(val.Count);
        Out.Show(temps.Count);
        Out.Show(val.Where(m => m.Won).Select(m => m.Player).Values);
        Out.Show(temps.Fold(0.0, (acc, t) => acc + t) / temps.Count);

        Out.Section("Méthode générique + contrainte (where T : ...)");
        Out.Show(MaxOf(new[] { 3, 9, 4 }));
        Out.Show(MaxOf(new[] { "pomme", "zèbre", "kiwi" }));
        Out.Show(Swap("x", "y"));

        Out.Section("Généricité de COMPORTEMENT : Func<T, TResult>");
        Func<Match, bool> isWin = m => m.Won;
        Func<double, bool> isAboveZero = d => d > 0;
        Out.Show(matches.Count(isWin));
        Out.Show(new[] { -1.0, 2, 3 }.Count(isAboveZero));
        Out.Note("Func<T,TResult> combine types génériques ET logique transportable.");
    }

    // 'where T : IComparable<T>' = T doit savoir se comparer
    static T MaxOf<T>(IEnumerable<T> source) where T : IComparable<T>
        => source.Aggregate((a, b) => a.CompareTo(b) >= 0 ? a : b);

    static (T, T) Swap<T>(T a, T b) => (b, a);
}
