namespace FpAide;

public record Point(double X, double Y);
public class PointClass { public double X { get; } public double Y { get; } public PointClass(double x, double y) { X = x; Y = y; } }
public record Team(string Name, List<string> Members);   // piège : List reste mutable !

public static class M06Purete
{
    static int _callCount = 0;

    public static void Run()
    {
        Out.Title("06 · Pureté & Immutabilité");

        Out.Section("Pure : même entrée → même sortie, aucun effet de bord, ne dépend que des arguments");
        Out.Show(Kda(10, 2, 5));
        Out.Show(Kda(10, 2, 5) == Kda(10, 2, 5));
        Out.Show(KdaWithNoise(10, 2, 5) == KdaWithNoise(10, 2, 5));   // false (presque sûrement)
        Out.Warn("Impure : Random, DateTime.Now, Console, fichier, BDD, variable globale, réseau.");

        KdaTracked(1, 1, 1); KdaTracked(1, 1, 1);
        Out.Show(_callCount);          // l'état global a changé → effet de bord

        Out.Section("Transparence référentielle : on peut remplacer l'appel par sa valeur");
        double result = Kda(10, 2, 5);
        Out.Show(result == 7.5);

        Out.Section("Mémoïsation (possible UNIQUEMENT parce que la fonction est pure)");
        int calls = 0;
        Func<int, long> slowSquare = n => { calls++; return (long)n * n; };
        var fast = Memoizer.Memoize(slowSquare);
        fast(12); fast(12); fast(12);
        Out.Show(calls);               // 1 seul vrai calcul
        Func<int, long>? fib = null;
        fib = Memoizer.Memoize<int, long>(n => n <= 1 ? n : fib!(n - 1) + fib!(n - 2));
        Out.Show(fib(80));             // instantané grâce au cache

        Out.Section("Immutabilité : on ne modifie pas, on crée une nouvelle valeur");
        var original = new[] { 1, 2, 3 };
        var doubled = original.Select(x => x * 2).ToArray();     // nouvelle collection
        Out.Show(original);
        Out.Show(doubled);

        Out.Section("Record : immutable + égalité par valeur + with");
        var p1 = new Point(1, 2);
        var p2 = p1 with { X = 5 };            // copie modifiée
        Out.Show(p1);
        Out.Show(p2);
        Out.Show(new Point(1, 2) == new Point(1, 2));                 // true  (record)
        Out.Show(new PointClass(1, 2) == new PointClass(1, 2));       // false (classe : compare les adresses)
        // p1.X = 9;  ← ERREUR DE COMPILATION (init-only)

        Out.Section("PIÈGE : readonly protège la RÉFÉRENCE, pas le contenu");
        var items = new List<int>();        // même avec 'readonly List<int>' en champ :
        items.Add(1);                       // le contenu reste modifiable !
        Out.Show(items);

        Out.Section("PIÈGE : record pas PROFONDÉMENT immuable");
        var t1 = new Team("A", new List<string> { "Léa" });
        var t2 = t1 with { Name = "B" };
        t2.Members.Add("Sam");              // modifie la MÊME liste !
        Out.Show(t1.Members);
        Out.Note("Solution : IReadOnlyList<string>, ImmutableList<T>, ou copier la liste.");

        Out.Section("Collections vraiment immuables (System.Collections.Immutable)");
        var il = System.Collections.Immutable.ImmutableList.Create(1, 2, 3);
        var il2 = il.Add(4);                // nouvelle liste
        Out.Show(il);
        Out.Show(il2);

        Out.Section("Fonction pure + injection de la source d'impureté");
        // Au lieu de DateTime.Now dans la fonction, on passe la date → testable
        Out.Show(IsWeekend(new DateTime(2026, 10, 10)));
    }

    static double Kda(int k, int d, int a) => (k + a) / (double)(d == 0 ? 1 : d);
    static double KdaWithNoise(int k, int d, int a) => Kda(k, d, a) * Random.Shared.NextDouble();
    static double KdaTracked(int k, int d, int a) { _callCount++; return Kda(k, d, a); }
    static bool IsWeekend(DateTime date) => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
}

public static class Memoizer
{
    public static Func<A, R> Memoize<A, R>(Func<A, R> f) where A : notnull
    {
        var cache = new Dictionary<A, R>();
        return a => cache.TryGetValue(a, out var r) ? r : (cache[a] = f(a));
    }
}
