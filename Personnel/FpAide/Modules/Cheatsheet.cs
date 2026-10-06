namespace FpAide;

public static class Cheatsheet
{
    public static void Run()
    {
        Out.Title("AIDE-MÉMOIRE");
        Console.WriteLine("""

   CONCEPT          LINQ                      SIGNATURE / IDÉE
   ─────────────────────────────────────────────────────────────────────────
   Filter           Where(p)                  T → bool           (N → ≤N)
   Map              Select(f)                 T → U              (N → N)
   Aplatir          SelectMany(f)             T → IEnumerable<U> (N → Σ)
   Fold             Aggregate(seed, f)        (Acc, T) → Acc     (N → 1)
   Grouper          GroupBy(key)              T → TKey → IGrouping
   Apparier         Zip(other, f)             (A, B) → C         (min des longueurs)
   Booléens         Any(p) / All(p)           Fold sur bool
   Ordonner         OrderBy / ThenBy / Desc   IOrderedEnumerable
   Découper         Take / Skip / First       paresseux (sauf First)
   Matérialiser     ToList/ToArray/ToDictionary/Count/Sum/Max/Min/Average

   TYPES DE FONCTIONS
   Action<A,B>          → void
   Func<A,B,TResult>    → le DERNIER type = le retour
   Func<T,bool>         → prédicat

   PIPELINE TYPE
   source.Where(…).Select(…).OrderBy(…).Take(n).ToList();

   TRIPLE RÈGLE
   Pure (déterministe, sans effet de bord, ne dépend que des args)
   + Immutable (record, with, pas de mutation)
   + Composable (f(g(x)) = x.g().f())

   RÉCURSION : cas de base + appel sur un problème plus petit + combinaison
   Liste plate → Aggregate ; arbre/répertoire → récursion explicite
   """);
        Out.Section("Squelettes à copier");
        Console.WriteLine("""
   // Extension
   public static class MyExt {
       public static IEnumerable<T> MyOp<T>(this IEnumerable<T> source, Func<T,bool> p) { ... }
   }

   // Récursion
   T Solve(Input x) => IsBase(x) ? BaseValue : Combine(x, Solve(Smaller(x)));

   // Fold par clé
   items.GroupBy(i => i.Key)
        .Select(g => new { g.Key, Total = g.Aggregate(0.0, (acc, i) => acc + i.Value) });

   // Record immuable
   public record Point(double X, double Y);   var p2 = p1 with { X = 5 };

   // Générateur de règles (capture)
   Func<int, Func<Player,bool>> minKda = min => p => p.Kda >= min;
   """);
    }
}
