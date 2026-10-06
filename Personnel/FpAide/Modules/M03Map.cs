namespace FpAide;

public static class M03Map
{
    public static void Run()
    {
        Out.Title("03 · Map / Select");
        var matches = Data.Matches();
        var people = Data.People();

        Out.Section("Select : N éléments → N éléments, source intacte");
        Out.Show(matches.Where(m => m.Deaths > 0).Select(m => (m.Kills + m.Assists) / (double)m.Deaths));
        Out.Note("Sans (double) : (18+6)/4 = division ENTIÈRE. Et deaths=0 → filtrer d'abord !");

        Out.Section("Extraire un attribut / calculer une valeur dérivée");
        Out.Show(people.Select(p => p.Name));
        Out.Show(people.Select(p => p.Sisters + p.Brothers));

        Out.Section("Changer le type de sortie");
        var temps = new[] { 25.0, 37.0 };
        Out.Show(temps.Select(t => t + 273.15));
        Out.Show(people.Take(2).Select(p => new { p.Name, Siblings = p.Sisters + p.Brothers }));   // classe anonyme
        Out.Show(people.Take(2).Select(p => new Member(p.Name, p.Age)));                           // type nommé

        Out.Section("Tuple : noms portés par la VARIABLE");
        (string Name, int Age) pair = ("Paul", 15);
        Out.Show(pair.Name);
        var pairs = people.Select(p => (p.Name, p.Age));        // type inféré (string Name, int Age)
        Out.Show(pairs.First().Name);
        List<(string, int)> noNames = pairs.ToList();           // noms perdus !
        Out.Show(noNames[0].Item1);
        Out.Show(pairs.Where(t => t.Age >= 18).Select(t => t.Name));

        Out.Section("Tuple/anonyme dans la méthode — type NOMMÉ dès que ça en sort");
        Out.Show(GetAdults().Select(a => a.Name));

        Out.Section("SelectMany : aplatir");
        var teams = new[]
        {
            new { Player = "Léa",     Matches = new[] { "m1", "m2", "m3" } },
            new { Player = "Raphaël", Matches = new[] { "m4", "m5" } },
        };
        Out.Show(teams.Select(t => t.Matches));        // imbriqué
        Out.Show(teams.SelectMany(t => t.Matches));    // aplati
        Out.Show(teams.SelectMany(t => t.Matches, (t, m) => $"{t.Player}:{m}"));  // avec résultat combiné
        Out.Show(Enumerable.Range(1, 3).SelectMany(a => Enumerable.Range(1, 2), (a, b) => (a, b)));  // produit cartésien

        Out.Section("Select avec index");
        Out.Show(people.Select((p, i) => $"{i + 1}. {p.Name}"));

        Out.Section("Capture : lue à l'EXÉCUTION");
        var players = Data.Players();
        double factor = 1.5;
        var boosted = players.Select(p => p.Kda * factor);
        factor = 2.0;
        Out.Show(boosted.ToList());     // × 2.0, pas × 1.5

        Out.Section("Pipeline : Where → Select → OrderBy → Take");
        Out.Show(players.Where(p => p.Games > 10).OrderByDescending(p => p.Kda).Select(p => p.Name).Take(2));
    }

    // Type nommé retourné par une méthode : record
    static IEnumerable<Adult> GetAdults() =>
        Data.People().Where(p => p.Age >= 18).Select(p => new Adult(p.Name, p.Age));
}
