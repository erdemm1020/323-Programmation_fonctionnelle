// EsportApp — analyse des performances de Team Helvetia (Valorant, CS2, LoL).
//
// Interface en ligne de commande construite à la main : aucune librairie
// externe, juste des comparaisons sur le tableau `args`.
//
// Chaque jeu a son propre modèle de match — qui porte sa date — sans type
// commun entre eux. Ce qui se partageait autrefois par une interface se passe
// désormais en paramètre, sous forme de fonctions.

using DataSeries;
using ESportApp;

const string version = "Erdem";

string[] knownFlags =
{
    "--help", "--version", "--game", "--player", "--filter"
};

// Les flags qui attendent une valeur juste après eux
string[] valueFlags =
{
    "--game", "--player", "--filter"
};

Console.WriteLine($"EsportApp v{version}");

// ─── Flags sans valeur ───────────────────────────────────────────────────────

if (args.Length == 0 || args.Contains("--help"))
{
    ShowHelp();
    return;
}

// ─── Validation de la ligne de commande ──────────────────────────────────────

string? unknownFlag = args.FirstOrDefault(a => a.StartsWith("--") && !knownFlags.Contains(a));
if (unknownFlag != null)
{
    Console.WriteLine($"Flag inconnu : {unknownFlag}");
    ShowHelp();
    return;
}

string? flagSansValeur = valueFlags.FirstOrDefault(f => args.Contains(f) && ValueOf(f) == null);
if (flagSansValeur != null)
{
    Console.WriteLine($"Le flag {flagSansValeur} attend une valeur.");
    ShowHelp();
    return;
}

// ─── Lecture des valeurs ─────────────────────────────────────────────────────

string? game = ValueOf("--game");
string? player = ValueOf("--player");
string filterMode = ValueOf("--filter") ?? "all";

// Tables de fonctions : le flag CLI choisit une fonction, pas un if/else.
// Ajouter un critère = ajouter une ligne dans la table.
// Les trois jeux n'ont aucun type commun : chacun a donc ses propres tables.
// Les formules se ressemblent — c'est le prix de modèles indépendants.
Dictionary<string, Func<ValorantMatch, bool>> valorantFilters = new Dictionary<string, Func<ValorantMatch, bool>>
{
    ["wins"] = m => m.Won,
    ["losses"] = m => !m.Won,
    ["all"] = m => true,
};

Dictionary<string, Func<Cs2Match, bool>> cs2Filters = new Dictionary<string, Func<Cs2Match, bool>>
{
    ["wins"] = m => m.Won,
    ["losses"] = m => !m.Won,
    ["all"] = m => true,
};

Dictionary<string, Func<LolMatch, bool>> lolFilters = new Dictionary<string, Func<LolMatch, bool>>
{
    ["wins"] = m => m.Won,
    ["losses"] = m => !m.Won,
    ["all"] = m => true,
};

string[] games = { "valorant", "cs2", "lol" };
string[] filterModes = { "wins", "losses", "all" };


if (!filterModes.Contains(filterMode))
{
    Console.WriteLine($"Filtre inconnu : {filterMode} (attendu : {string.Join(", ", filterModes)})");
    return;
}

// ─── Chargement des données ──────────────────────────────────────────────────

// TODO 04: Ajouter le traitement d'un argument supplémentaire '--folder' qui permet de spécifier dans quel dossier sont les fichiers CSV
//          Si le paramètre n'est pas spécifié: prendre le répertoire courant (Directory.GetCurrentDirectory())
//          Lire les fichiers CSV dans le dossier spécifié


DataSerie<ValorantMatch> valorant =
    DataSerie<ValorantMatch>.FromCsv(@"data/valorant.csv", ParseValorant);
DataSerie<Cs2Match> cs2 =
    DataSerie<Cs2Match>.FromCsv(@"data/cs2.csv", ParseCS2);
DataSerie<LolMatch> lol =
    DataSerie<LolMatch>.FromCsv(@"data/lol.csv", ParseLoL);

// TODO 05: Lire plusieurs fichiers pour un jeu. Par exemple 'lol-2025.csv' et 'lol-2026.csv'.
//          Tous les fichiers dont le nom commence par le nom du jeu doivent être lus.
//          Utiliser Directory.GetFiles()
//          L'utilisation d'une boucle 'foreach(...)' est autorisée

// TODO 06: Refactoriser avec SelectMany pour faire disparaître la boucle foreach

// TODO 00: Changer le numéro de version dans le help et faire un commit de départ


// TODO 3

    switch (filterMode)
    {
        case "wins":
            valorant = DataSerie<ValorantMatch>.From(valorant.Values.Where(vw => vw.Won));
            cs2 = DataSerie<Cs2Match>.From(cs2.Values.Where(cw => cw.Won));
            lol = DataSerie<LolMatch>.From(lol.Values.Where(lw => lw.Won));
            break;
        
        case "losses":
            valorant = DataSerie<ValorantMatch>.From(valorant.Values.Where(vw => !vw.Won));
            cs2 = DataSerie<Cs2Match>.From(cs2.Values.Where(cw => !cw.Won));
            lol = DataSerie<LolMatch>.From(lol.Values.Where(lw => !lw.Won));
            break;
        default:
            break;
    }


// TODO 2

if (player != null)
{
    valorant = DataSerie<ValorantMatch>.From(valorant.Values.Where(vp => vp.Player == player));
    cs2 = DataSerie<Cs2Match>.From(cs2.Values.Where(csp => csp.Player == player));
    lol = DataSerie<LolMatch>.From(lol.Values.Where(lmp => lmp.Player == player));

}


// TODO 1

if (game != null && !games.Contains(game))
{
    Console.WriteLine($"Jeu inconnu : {game} (attendu : {string.Join(", ", games)})");
    return;
}
else
{
    Console.WriteLine("Jeu selectionnée : {0}", game);

    switch (game)
    {
        case "valorant":
            Console.WriteLine(valorant);
            break;
        case "cs2":
            Console.WriteLine(cs2);
            break;
        case "lol":
            Console.WriteLine(lol);
            break;

         default:
            Console.WriteLine(valorant);
            Console.WriteLine(cs2);
            Console.WriteLine(lol);
            break;
    }
}







Console.WriteLine("That's all folks!");

// ─── Fonctions ───────────────────────────────────────────────────────────────

void ShowHelp()
{
    Console.WriteLine("Usage: EsportApp [options]");
    Console.WriteLine();
    Console.WriteLine("  Analyse des performances de Team Helvetia (Valorant, CS2, LoL).");
    Console.WriteLine();
    Console.WriteLine("Sélection des données");
    Console.WriteLine("  --game   valorant|cs2|lol    Jeu à analyser              (défaut : les trois)");
    Console.WriteLine("  --player <nom>               Restreindre à un joueur     (défaut : tous)");
    Console.WriteLine("  --filter wins|losses|all     Issue des matchs retenus    (défaut : all)");
    Console.WriteLine();
    Console.WriteLine("Divers");
    Console.WriteLine("  --help                       Affiche cette aide");
    Console.WriteLine("  --version                    Affiche la version");
    Console.WriteLine(Environment.NewLine+"version formative, ...");

}

// Retourne la valeur qui suit un flag, ou null si le flag est absent ou si
// aucune valeur ne le suit.
string? ValueOf(string flag)
{
    int i = Array.IndexOf(args, flag);
    if (i < 0 || i + 1 >= args.Length || args[i + 1].StartsWith("--"))
        return null;
    return args[i + 1];
}

// ─── Parsers : le domaine est ici, la bibliothèque l'ignore ──────────────────

ValorantMatch ParseValorant(string[] cols)
{
    return new ValorantMatch(DateTime.Parse(cols[0]), cols[1], cols[2],
        int.Parse(cols[3]), int.Parse(cols[4]), int.Parse(cols[5]),
        int.Parse(cols[6]), int.Parse(cols[7]), bool.Parse(cols[8]));
}

Cs2Match ParseCS2(string[] cols)
{
    return new Cs2Match(DateTime.Parse(cols[0]), cols[1], cols[2], cols[3],
        int.Parse(cols[4]), int.Parse(cols[5]), int.Parse(cols[6]),
        int.Parse(cols[7]), bool.Parse(cols[8]));
}

LolMatch ParseLoL(string[] cols)
{
    return new LolMatch(DateTime.Parse(cols[0]), cols[1], cols[2],
        int.Parse(cols[4]), int.Parse(cols[5]), int.Parse(cols[6]),
        int.Parse(cols[7]), int.Parse(cols[8]), bool.Parse(cols[9]));
}

