namespace FpAide;

public static class Pieges
{
    public static void Run()
    {
        Out.Title("PIÈGES CLASSIQUES DU TEST");

        Out.Section("1. Évaluation paresseuse : la requête est ré-exécutée à CHAQUE énumération");
        int executions = 0;
        var q = new[] { 1, 2, 3 }.Select(n => { executions++; return n * 2; });
        q.Count(); q.ToList(); q.Sum();
        Out.Show(executions);                 // 9 et non 3 !
        var snapshot = q.ToList();            // ToList() = figer le résultat
        Out.Note("Solution : matérialiser UNE fois avec ToList()/ToArray() si on réutilise.");

        Out.Section("2. Captures : 'for' partage UNE variable, 'foreach' en crée une par tour");
        var fs = new List<Func<int>>();
        for (int i = 0; i < 3; i++) fs.Add(() => i);
        Out.Show(fs.Select(f => f()));        // [3, 3, 3]
        var gs = new List<Func<int>>();
        foreach (var j in Enumerable.Range(0, 3)) gs.Add(() => j);
        Out.Show(gs.Select(g => g()));        // [0, 1, 2]
        Out.Note("Correction dans un for : copier dans une variable locale (int copy = i;).");

        Out.Section("3. Capture + paresse : la valeur est lue à l'EXÉCUTION");
        int threshold = 1;
        var big = new[] { 1, 2, 3 }.Where(n => n > threshold);
        threshold = 2;
        Out.Show(big.ToList());               // [3] et pas [2, 3]

        Out.Section("4. Division entière");
        int k = 7, d = 2;
        Out.Show(k / d);                      // 3
        Out.Show(k / (double)d);              // 3.5
        Out.Show((double)(k / d));            // 3 ← le cast arrive TROP TARD

        Out.Section("5. Collections vides");
        var empty = new int[0];
        Out.Show(empty.Sum());                // 0
        Out.Try(() => empty.Max());           // exception
        Out.Try(() => empty.Average());       // exception
        Out.Try(() => empty.Aggregate((a, b) => a + b));   // exception (pas de seed)
        Out.Show(empty.Aggregate(0, (a, b) => a + b));     // OK avec seed
        Out.Show(empty.All(x => x > 0));      // true  (vacuité)
        Out.Show(empty.Any());                // false

        Out.Section("6. First / FirstOrDefault / Single");
        Out.Try(() => empty.First());
        Out.Show(empty.FirstOrDefault());     // 0 (default)
        Out.Try(() => new[] { 1, 2 }.Single());

        Out.Section("7. Zip s'arrête à la plus courte (silencieusement)");
        Out.Show(new[] { 1, 2, 3 }.Zip(new[] { "a", "b" }));

        Out.Section("8. Tuples : les noms disparaissent si le type est déclaré sans noms");
        var named = new[] { ("Paul", 15) }.Select(t => (Name: t.Item1, Age: t.Item2));
        Out.Show(named.First().Name);
        List<(string, int)> unnamed = named.ToList();
        Out.Show(unnamed[0].Item1);           // .Name n'existe plus

        Out.Section("9. GroupBy / Select : types retournés");
        Out.Note("Select → IEnumerable<IEnumerable<T>> (imbriqué) ; SelectMany → IEnumerable<T> (aplati).");
        Out.Note("GroupBy → IEnumerable<IGrouping<TKey,T>>, PAS un Dictionary (ToDictionary pour convertir).");
        Out.Note("OrderBy retourne IOrderedEnumerable<T> ; ThenBy pour un 2e critère.");

        Out.Section("10. Modifier une collection pendant un foreach");
        Out.Try(() =>
        {
            var l = new List<int> { 1, 2, 3 };
            foreach (var x in l) l.Add(x);    // InvalidOperationException
            return l.Count;
        });

        Out.Section("11. readonly ≠ immuable ; record ≠ profondément immuable");
        Out.Note("readonly = référence figée, contenu modifiable. record avec List<T> : 'with' copie la RÉFÉRENCE.");

        Out.Section("12. Récursion");
        Out.Note("Pas de cas de base (ou cas jamais atteint) → StackOverflowException.");
        Out.Note("Fibonacci naïf : complexité exponentielle → mémoïser ou utiliser Aggregate.");
        Out.Note("C# ne garantit pas la TCO : pour de grandes listes, préférer Aggregate / boucle.");
    }
}
