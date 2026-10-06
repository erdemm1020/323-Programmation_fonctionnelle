namespace FpAide;

public static class M05Extensions
{
    public static void Run()
    {
        Out.Title("05 · Extensions, DSL, Zip, Composition");

        Out.Section("Méthode d'extension (voir Extensions.cs : static class + this)");
        Out.Show("Bob".Greetings());
        Out.Show("  hello world ".Trim().RemoveSpaces().ToUpper());

        Out.Section("Chaînage fluide : les données d'abord");
        Data.Matches()
            .Where(m => m.Won)
            .Select(m => m.Player)
            .JoinWith(", ")
            .ToConsole();

        Out.Section("Extension qui retourne IEnumerable<T> → chaînable");
        var data = new[] { "BoB", "Max", "jOelLe", "NadiA" };
        Out.Show(data.Where(n => n.StartsWith("j")).ToLower().ToList());
        Out.Show(data.ToLower(random: true).ToList());

        Out.Section("Mes propres Where / Select / Fold (voir Extensions.cs)");
        var nums = new[] { 1, 2, 3, 4, 5 };
        Out.Show(nums.MyWhere(n => n % 2 == 1).MySelect(n => n * 10));
        Out.Show(nums.MyFold(0, (acc, n) => acc + n));

        Out.Section("Zip : appariement par POSITION, arrêt sur la plus courte");
        var dates = new[] { "01.03", "08.03", "15.03" };
        var kda = new[] { 2.4, 3.1, 1.8, 2.9 };      // 4 valeurs → la dernière est ignorée
        Out.Show(dates.Zip(kda, (d, k) => $"{d} → KDA {k}"));
        Out.Show(dates.Zip(kda));                     // sans combineur : tuples
        Out.Show(kda.Zip(kda.Skip(1), (a, b) => b - a));   // différences successives (évolution)

        Out.Section("DSL : un code qui parle le métier");
        Out.Show(Data.Players().Where(p => p.IsTopPlayer()).Select(p => p.Name));
        Out.Show(Data.Players().Where(p => p.HasPlayedAtLeast(30)).Select(p => p.Name));

        Out.Section("Composition de fonctions : f ∘ g");
        Func<string, string> trim = s => s.Trim();
        Func<string, string> toLower = s => s.ToLower();
        Func<string, string> normalize = s => toLower(trim(s));        // à la main
        Out.Show(normalize("  HELLO  "));
        var normalize2 = trim.Then(toLower);                           // avec Then : trim PUIS toLower
        Out.Show(normalize2("  WORLD "));
        Out.Show("  ABC ".Pipe(trim).Pipe(toLower));                   // Pipe

        Out.Section("Ordre de composition");
        Func<int, int> plus1 = x => x + 1;
        Func<int, int> times2 = x => x * 2;
        Out.Show(plus1.Then(times2)(5));    // (5+1)*2 = 12
        Out.Show(times2.Then(plus1)(5));    // (5*2)+1 = 11
    }
}

// DSL : méthodes d'extension qui nomment des règles métier
public static class PlayerDsl
{
    public static bool IsTopPlayer(this Player p) => p.Kda > 2;
    public static bool HasPlayedAtLeast(this Player p, int games) => p.Games >= games;
}
