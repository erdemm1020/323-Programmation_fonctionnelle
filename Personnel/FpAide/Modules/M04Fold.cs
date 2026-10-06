namespace FpAide;

public static class M04Fold
{
    public static void Run()
    {
        Out.Title("04 · Fold / Aggregate / GroupBy");
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var people = Data.People();

        Out.Section("Agrégateurs courants (avec sélecteur sur objets)");
        Out.Show(numbers.Sum());
        Out.Show(numbers.Average());
        Out.Show(numbers.Max());
        Out.Show(numbers.Min());
        Out.Show(numbers.Count());
        Out.Show(people.Average(p => p.Age));
        Out.Show(people.Min(p => p.Age));
        Out.Show(people.MaxBy(p => p.Age)!.Name);     // MaxBy = l'ÉLÉMENT, Max = la valeur

        Out.Section("Aggregate : seed + f(acc, élément)");
        Out.Show(numbers.Aggregate((acc, v) => acc + v));            // sans seed (1er élément = seed)
        Out.Show(numbers.Aggregate(0, (acc, v) => acc + v));         // avec seed
        Out.Show(numbers.Aggregate(0, (acc, v) => acc + v, total => $"Somme : {total}"));   // + résultat transformé

        Out.Section("Trace de l'accumulateur");
        numbers.Aggregate(0, (acc, v) => { Console.WriteLine($"      acc={acc} + {v} → {acc + v}"); return acc + v; });

        Out.Section("Tout est un Fold");
        Out.Show(numbers.Aggregate(0, (acc, n) => acc + n));                             // Sum
        Out.Show(numbers.Aggregate(0, (acc, _) => acc + 1));                             // Count
        Out.Show(numbers.Aggregate(int.MinValue, (acc, n) => n > acc ? n : acc));        // Max
        Out.Show(numbers.Aggregate(int.MaxValue, (acc, n) => n < acc ? n : acc));        // Min
        Out.Show(numbers.Aggregate(false, (acc, n) => acc || n % 2 == 0));               // Any
        Out.Show(numbers.Aggregate(true, (acc, n) => acc && n > 0));                     // All
        Out.Show(numbers.Aggregate(1, (acc, n) => acc * n));                             // Produit (factorielle !)
        Out.Show(numbers.Aggregate("", (acc, n) => acc + n));                            // concaténation

        Out.Section("Imiter Map avec Fold (illustration, pas équivalence)");
        Out.Show(numbers.Aggregate(new List<int>(), (acc, v) => { acc.Add(v * 2); return acc; }));
        Out.Show(numbers.Select(n => n * 2));   // la vraie façon

        Out.Section("Fold avec un accumulateur composé (tuple) : min ET max en un passage");
        var (min, max) = numbers.Aggregate((min: int.MaxValue, max: int.MinValue),
            (acc, n) => (Math.Min(acc.min, n), Math.Max(acc.max, n)));
        Out.Show((min, max));

        Out.Section("Dictionary<TKey,TValue> (outil requis pour GroupBy)");
        Dictionary<string, int> ages = new() { ["Alice"] = 30, ["Bob"] = 25 };
        Out.Show(ages["Bob"]);
        Out.Show(ages.ContainsKey("Bob"));
        Out.Show(ages.TryGetValue("Zoé", out var age));
        Out.Show(age);

        Out.Section("GroupBy : partitionner par clé (IGrouping<TKey,T>)");
        var groups = people.GroupBy(p => p.Sisters + p.Brothers).OrderBy(g => g.Key);
        Out.Show(groups.Select(g => $"{g.Key} → {string.Join(",", g.Select(p => p.Name))}"));
        Out.Show(groups.Select(g => new { FamilySize = g.Key, Members = g.Select(p => p.Name).ToList() }));

        Out.Section("GroupBy + Fold par clé");
        Out.Show(people.GroupBy(p => p.Sisters + p.Brothers)
                       .OrderBy(g => g.Key)
                       .Select(g => new { FamilySize = g.Key, AvgAge = g.Aggregate(0.0, (acc, p) => acc + p.Age) / g.Count() }));
        Out.Show(people.GroupBy(p => p.Sisters + p.Brothers).ToDictionary(g => g.Key, g => g.Count()));

        Out.Section("GroupBy à la main (impératif) vs GroupBy");
        var manual = new Dictionary<int, List<Person>>();
        foreach (var p in people)
        {
            int key = p.Sisters + p.Brothers;
            if (!manual.ContainsKey(key)) manual[key] = new List<Person>();
            manual[key].Add(p);
        }
        Out.Show(manual.Select(kv => $"{kv.Key} → {kv.Value.Count}"));
        var dict = people.GroupBy(p => p.Sisters + p.Brothers).ToDictionary(g => g.Key, g => g.ToList());
        Out.Show(dict[1].Select(p => p.Name));

        Out.Section("Triptyque Filter → Map → Fold");
        Out.Show(Data.Matches()
            .Where(m => m.Deaths > 0)                                   // Filter
            .Select(m => (m.Kills + m.Assists) / (double)m.Deaths)      // Map
            .Aggregate(0.0, (acc, k) => acc + k));                      // Fold
    }
}
