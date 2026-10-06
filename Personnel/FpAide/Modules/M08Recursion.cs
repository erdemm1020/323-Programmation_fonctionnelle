namespace FpAide;

public record Doll(Doll? Inner);
public record FileItem(string Name, long Size);
public record Dir(string Name, List<FileItem> Files, List<Dir> SubDirs);
public class Node { public int Value; public Node? Left, Right; public Node(int v, Node? l = null, Node? r = null) { Value = v; Left = l; Right = r; } }

public static class M08Recursion
{
    public static void Run()
    {
        Out.Title("08 · Récursivité");
        Out.Note("Schéma : 1) CAS DE BASE (trivial)  2) APPEL RÉCURSIF sur un problème PLUS PETIT + combinaison.");

        Out.Section("Factorielle : itérative vs récursive");
        Out.Show(FactIter(6));
        Out.Show(Fact(6));
        Out.Show(FactTail(6));
        Out.Show(Enumerable.Range(1, 6).Aggregate(1, (acc, n) => acc * n));   // en Fold

        Out.Section("Poupées russes (cas de base : plus de poupée)");
        var grande = new Doll(new Doll(new Doll(new Doll(null))));
        Out.Show(CountDolls(grande));

        Out.Section("Somme récursive sur une liste (cas de base : liste vide)");
        Out.Show(Sum(new[] { 1, 2, 3, 4, 5 }));
        Out.Show(SumFold(new[] { 1, 2, 3, 4, 5 }));
        Out.Note("Sum récursif avec Skip(1) = O(n²) et risque de stack overflow. Fold = bien meilleur sur une liste.");

        Out.Section("Fibonacci");
        Out.Show(Enumerable.Range(0, 10).Select(n => Fib(n)));                          // récursif naïf (exponentiel !)
        Out.Show(Enumerable.Range(0, 10).Select(n => FibFold(n)));                      // en Fold, O(n)
        Out.Show(FibFold(50));

        Out.Section("Autres classiques de test");
        Out.Show(Gcd(48, 18));                       // Euclide
        Out.Show(Power(2, 10));
        Out.Show(Reverse("bonjour"));
        Out.Show(IsPalindrome("kayak"));
        Out.Show(Flatten(new List<object> { 1, new List<object> { 2, 3, new List<object> { 4 } }, 5 }));
        Out.Show(Digits(12345));

        Out.Section("Structure ARBORESCENTE : taille d'un répertoire");
        var projet = new Dir("projet",
            new() { new("README.md", 1) },
            new()
            {
                new Dir("src",
                    new() { new("main.cs", 5) },
                    new() { new Dir("utils", new() { new("helper.cs", 2) }, new()) })
            });
        Out.Show(Size(projet));
        Out.Show(ListFiles(projet, ""));

        Out.Section("Arbre binaire : somme, hauteur, contient");
        //        5
        //       / \
        //      3   8
        //     /
        //    1
        var tree = new Node(5, new Node(3, new Node(1)), new Node(8));
        Out.Show(TreeSum(tree));
        Out.Show(Height(tree));
        Out.Show(Contains(tree, 8));

        Out.Section("Récursion vs Fold");
        Out.Note("Liste plate → Fold (Aggregate). Structure arborescente → récursion explicite.");
        Out.Warn("Le cours dit que Aggregate applique la TCO : en réalité .NET ne garantit PAS la TCO pour C#.");
        Out.Note("Aggregate est simplement une boucle (itératif) → pas de pile qui grandit. À formuler ainsi au test.");
    }

    // ---- Factorielle ----
    static int FactIter(int x) { int res = 1; for (int i = x; i > 1; i--) res *= i; return res; }
    static int Fact(int x) => x <= 1 ? 1 : x * Fact(x - 1);                  // cas de base + règle
    static int FactTail(int x, int acc = 1) => x <= 1 ? acc : FactTail(x - 1, x * acc);   // tail-call (acc = résultat partiel)

    // ---- Listes ----
    static int CountDolls(Doll? d) => d is null ? 0 : 1 + CountDolls(d.Inner);
    static int Sum(IEnumerable<int> list) => !list.Any() ? 0 : list.First() + Sum(list.Skip(1));
    static int SumFold(IEnumerable<int> list) => list.Aggregate(0, (acc, v) => acc + v);

    // ---- Fibonacci ----
    static long Fib(int n) => n <= 1 ? n : Fib(n - 1) + Fib(n - 2);
    static long FibFold(int n) => Enumerable.Range(0, n).Aggregate((a: 0L, b: 1L), (t, _) => (t.b, t.a + t.b)).a;

    // ---- Classiques ----
    static int Gcd(int a, int b) => b == 0 ? a : Gcd(b, a % b);
    static long Power(long b, int e) => e == 0 ? 1 : b * Power(b, e - 1);
    static string Reverse(string s) => s.Length <= 1 ? s : Reverse(s[1..]) + s[0];
    static bool IsPalindrome(string s) => s.Length <= 1 || (s[0] == s[^1] && IsPalindrome(s[1..^1]));
    static int Digits(int n) => n < 10 ? n : n % 10 + Digits(n / 10);       // somme des chiffres
    static List<int> Flatten(List<object> list) => list.SelectMany(o => o is List<object> sub ? Flatten(sub) : new List<int> { (int)o }).ToList();

    // ---- Répertoires ----
    static long Size(Dir d) => d.Files.Sum(f => f.Size) + d.SubDirs.Sum(s => Size(s));
    static IEnumerable<string> ListFiles(Dir d, string path)
    {
        var here = $"{path}{d.Name}/";
        return d.Files.Select(f => here + f.Name).Concat(d.SubDirs.SelectMany(s => ListFiles(s, here)));
    }

    // ---- Arbres ----
    static int TreeSum(Node? n) => n is null ? 0 : n.Value + TreeSum(n.Left) + TreeSum(n.Right);
    static int Height(Node? n) => n is null ? 0 : 1 + Math.Max(Height(n.Left), Height(n.Right));
    static bool Contains(Node? n, int v) => n is not null && (n.Value == v || Contains(n.Left, v) || Contains(n.Right, v));
}
