using DataSeries;
using ESport;

DataSeries<DataPoint<ValorantMatch>> valorant;
DataSeries<DataPoint<Cs2Match>> cs2;
DataSeries<DataPoint<LolMatch>> lol;

valorant = DataSeries<DataPoint<ValorantMatch>>.FromCsv("data/valorant.csv", ParseValorant);
lol = DataSeries<DataPoint<LolMatch>>.FromCsv("data/lol.csv", ParseLol);
cs2 = DataSeries<DataPoint<Cs2Match>>.FromCsv("data/cs2.csv", ParseCs2);

// Console.WriteLine($"Valorant match: {valorant.Count}");
// Console.WriteLine($"LoL match: {lol.Count}");
// Console.WriteLine($"CS2 match: {cs2.Count}");


// 1. Déclaration compatible
DataSeries<DataPoint<Cs2Match>> raphaelGenerated = DataSeries<DataPoint<Cs2Match>>.From(
    MatchGenerator.GenerateCs2("Raphaël", 20)
);

Console.WriteLine("{0} matchs générés", raphaelGenerated.Count); // 20

Func<Cs2Match, bool> isValid = m =>
    m.Kills + m.Assists <= 50 &&
    m.Deaths >= 1;



static void ExportCSV(DataSeries<DataPoint<Cs2Match>> series, string path) =>
    File.WriteAllLines(
        path,
        series.Values
            .Select(dp => $"{dp.Timestamp:yyyy-MM-dd},{dp.Value.Player},{dp.Value.Map},{dp.Value.StartSide},{dp.Value.Kills},{dp.Value.Deaths},{dp.Value.Assists},{dp.Value.Mvps},{dp.Value.Won.ToString().ToLower()}")
            .Prepend("date,player,map,start_side,kills,deaths,assists,mvps,won")
    );

ExportCSV(raphaelGenerated, "test.csv");


// var raphaelValid = raphaelGenerated.Filter(isValid);
// Console.WriteLine($"Avant : {raphaelGenerated.Count}, après : {raphaelValid.Count}");
Environment.Exit(67);


DataPoint<ValorantMatch> ParseValorant(string[] cols)
{
    ValorantMatch match = new ValorantMatch(
        cols[1], // Player
        cols[2], // Agent
        int.Parse(cols[3]), // Kills
        int.Parse(cols[4]), // Deaths
        int.Parse(cols[5]), // Assists
        int.Parse(cols[6]), // Headshots
        int.Parse(cols[7]), // Damage
        bool.Parse(cols[8])  // Win
    );
    
    DateTime date = DateTime.Parse(cols[0]); // Date

    return new DataPoint<ValorantMatch>(date, match);
}

DataPoint<LolMatch> ParseLol(string[] cols)
{
    LolMatch match = new LolMatch(
        cols[1], // Player
        cols[2], // Champion
        cols[3], // Role
        int.Parse(cols[4]), // Kills
        int.Parse(cols[5]), // Deaths
        int.Parse(cols[6]), // Assists
        int.Parse(cols[7]), // CS
        int.Parse(cols[8]), // Vision Score
        bool.Parse(cols[9])  // Win
    );
    
    DateTime date = DateTime.Parse(cols[0]); // Date

    return new DataPoint<LolMatch>(date, match);
}


DataPoint<Cs2Match> ParseCs2(string[] cols)
{
    Cs2Match match = new Cs2Match(
        cols[1], // Player
        cols[2], // Map
        cols[3], // Side
        int.Parse(cols[4]), // Kills
        int.Parse(cols[5]), // Deaths
        int.Parse(cols[6]), // Assists
        int.Parse(cols[7]), // Headshots
        bool.Parse(cols[8])  // Win
    );
    
    DateTime date = DateTime.Parse(cols[0]); // Date

    return new DataPoint<Cs2Match>(date, match);
}