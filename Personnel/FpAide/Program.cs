using System.Globalization;
using System.Text;
using FpAide;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var topics = new List<Topic>
{
    new("1", "01 · Paradigmes & Généricité (DataSeries<T>)", "impératif déclaratif généricité generic dataseries T list func where contrainte", M01Paradigmes.Run),
    new("2", "02 · HOF & Filter (Action, Func, lambda, capture, Where, lazy, Any/All)", "action func lambda capture where filter paresseux lazy any all prédicat", M02Filter.Run),
    new("3", "03 · Map / Select (projection, tuple, anonyme, SelectMany)", "map select projection tuple anonyme selectmany aplatir pipeline record", M03Map.Run),
    new("4", "04 · Fold / Aggregate / GroupBy / Dictionary", "fold aggregate reduce sum max min count average groupby dictionary seed todictionary", M04Fold.Run),
    new("5", "05 · Extensions, DSL, Zip, composition", "extension this dsl zip composition chaînage fluent pipe then", M05Extensions.Run),
    new("6", "06 · Pureté & Immutabilité (record, with, mémoïsation)", "pure pureté immutabilité record with readonly mémoïsation memoize référentielle effet de bord", M06Purete.Run),
    new("8", "08 · Récursivité (factorielle, Fibonacci, arbres, TCO)", "récursion recursivité factorielle fibonacci arbre tco tail cas de base répertoire", M08Recursion.Run),
    new("p", "Pièges classiques du test", "piège piege erreur lazy capture division vide exception", Pieges.Run),
    new("c", "Aide-mémoire + squelettes à copier", "cheat aide mémoire résumé signature", Cheatsheet.Run),
};

while (true)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔════════════════════════════════════════════╗");
    Console.WriteLine("║  FP Aide-test · C# & LINQ fonctionnel      ║");
    Console.WriteLine("╚════════════════════════════════════════════╝");
    Console.ResetColor();
    foreach (var t in topics) Console.WriteLine($"  [{t.Key}] {t.Title}");
    Console.WriteLine("  [a] Tout exécuter     [q] Quitter");
    Console.WriteLine("  ou tape un MOT-CLÉ pour chercher (ex: capture, groupby, zip, record…)");
    Console.Write("\n> ");
    var input = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    if (input is "q" or "quit" or "exit") break;
    if (input == "") continue;

    if (input == "a")
    {
        foreach (var t in topics) t.Run();
    }
    else if (topics.FirstOrDefault(t => t.Key == input) is { } exact)
    {
        exact.Run();
    }
    else
    {
        var hits = topics.Where(t => t.Tags.Contains(input) || t.Title.ToLowerInvariant().Contains(input)).ToList();
        if (hits.Count == 0) { Console.WriteLine($"Aucun résultat pour « {input} »."); }
        else
        {
            Console.WriteLine($"Résultats pour « {input} » : " + string.Join(", ", hits.Select(h => $"[{h.Key}]")));
            foreach (var h in hits) h.Run();
        }
    }
    Console.WriteLine("\n[Entrée] pour revenir au menu…");
    Console.ReadLine();
}

record Topic(string Key, string Title, string Tags, Action Run);
