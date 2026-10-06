namespace FpAide;

public static class M02Filter
{
    public static void Run()
    {
        Out.Title("02 · Fonctions d'ordre supérieur & Filter");

        Out.Section("Action (void) et Func (retourne — DERNIER type = retour)");
        Action<string> log = s => Console.WriteLine("   LOG: " + s);
        log("Match terminé !");
        Func<int, int, double> divide = (a, b) => (double)a / b;
        Out.Show(divide(10, 2));
        Compute(10, 4, (a, b) => a * b);              // passer un comportement en paramètre

        Out.Section("Changer d'implémentation sans toucher au code appelant");
        Action<string> output = ToConsole;            // ← seule ligne à changer
        output("Démarrage");
        static void ToConsole(string t) => Console.WriteLine("   [console] " + t);

        Out.Section("Lambdas : paramètres à gauche de =>, corps à droite");
        Func<int> one = () => 1;
        Func<int, int> x2 = x => x * 2;
        Func<int, int, int> add = (x, y) => x + y;
        Func<int, bool> isEven = x => x % 2 == 0;
        Out.Show(one());
        Out.Show(x2(21));
        Out.Show(add(2, 3));
        Out.Show(isEven(4));
        Action<int> print = x => { Console.WriteLine($"   Valeur : {x}"); };   // corps multi-lignes
        print(42);

        Out.Section("Where : un tamis (le prédicat est une valeur)");
        var players = Data.Players();
        Out.Show(players.Where(p => p.Kda > 2).Select(p => p.Name));
        Out.Show(players.Where(p => p.Rank > 10).Select(p => p.Name));
        Out.Show(players.Where(p => p.Games > 50).Select(p => p.Name));
        Out.Show(players.Where(p => p.Kda > 2).Where(p => p.Games > 10).Select(p => p.Name));  // chaînés
        Out.Show(players.Where(p => p.Kda > 2 && p.Games > 10).Select(p => p.Name));           // &&

        Out.Section("Captures : le lambda garde un LIEN vers la variable, pas une copie");
        int minKda = 2;
        Func<Player, bool> isGood = p => p.Kda >= minKda;
        Out.Show(players.Where(isGood).Select(p => p.Name));
        minKda = 3;                                    // la "boîte" change...
        Out.Show(players.Where(isGood).Select(p => p.Name));   // ...le lambda voit 3

        Out.Section("Générateur de règles (lambda qui retourne un lambda)");
        Func<int, Func<Player, bool>> minKdaFilter = min => p => p.Kda >= min;
        var top = minKdaFilter(3);
        var good = minKdaFilter(2);
        Out.Show(players.Where(top).Select(p => p.Name));
        Out.Show(players.Where(good).Select(p => p.Name));

        Out.Section("Évaluation PARESSEUSE : Where décrit, ToList() exécute");
        var list = new List<Player> { new("Léa", 2.4, 8, 60), new("Sam", 1.2, 15, 30) };
        var query = list.Where(p => p.Kda > 2);        // RIEN n'est filtré ici
        list.Add(new Player("Raph", 3.1, 20, 40));     // ajouté APRÈS la construction
        Out.Show(query.Select(p => p.Name).ToList());  // Raph est inclus !

        var nums = new[] { 1, 2, 3, 4 };
        var spy = nums.Where(n => { Console.WriteLine($"      (test de {n})"); return n > 2; });
        Console.WriteLine("   → avant ToList : rien n'a été testé");
        var res = spy.ToList();
        Console.WriteLine("   → après ToList : tests exécutés");
        Out.Note("Paresseux : Where, Select, OrderBy, Skip, Take. Immédiat : ToList, ToArray, Count, Sum, Max, First, Any.");

        Out.Section("Any / All (Fold sur des booléens)");
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        Out.Show(numbers.Any(n => n % 2 == 0));
        Out.Show(numbers.Any(n => n < 0));
        Out.Show(numbers.Any());
        Out.Show(numbers.All(n => n > 0));
        Out.Show(numbers.All(n => n % 2 == 0));
        Out.Note("All sur liste VIDE → true ; Any sur liste VIDE → false.");
        Out.Show(new List<int>().All(n => n > 100));

        Out.Section("Ma propre version de Where (yield) — voir Extensions.cs");
        Out.Show(numbers.MyWhere(n => n > 2));
    }

    static void Compute(int a, int b, Func<int, int, int> operation)
        => Console.WriteLine($"   Compute({a},{b}) → {operation(a, b)}");
}
