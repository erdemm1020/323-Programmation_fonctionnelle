namespace FpAide;

// Les "records" = types immuables avec égalité par valeur (thématique 06)
public record Player(string Name, double Kda, int Rank, int Games);
public record Match(string Player, int Kills, int Deaths, int Assists, bool Won);
public record Person(string Name, int Age, int Sisters, int Brothers);
public record Member(string Name, int Age);
public record Adult(string Name, int Age);

/// <summary>Jeux de données utilisés dans toutes les démos.</summary>
public static class Data
{
    public static List<Player> Players() => new()
    {
        new("Léa",  2.4,  8, 60),
        new("Sam",  1.2, 15, 30),
        new("Raph", 3.1, 20, 40),
        new("Kai",  0.8,  3,  5),
    };

    public static List<Match> Matches() => new()
    {
        new("Léa",  18, 4, 6, true),
        new("Raph", 12, 5, 9, true),
        new("Sam",   5, 0, 3, false),   // deaths = 0 → attention division par zéro
        new("Kai",   8, 10, 2, false),
    };

    public static List<Person> People() => new()
    {
        new("Paul",     15, 2, 1),
        new("Lucie",    18, 1, 3),
        new("Claude",   17, 0, 0),
        new("Germaine", 18, 1, 0),
        new("Pierre",   19, 0, 1),
        new("Sylvie",   17, 1, 0),
    };
}
