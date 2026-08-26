using DataSeries;
using ESport;

var valorant = DataSeries<ValorantMatch>.From(new[]
{
    new ValorantMatch("Léa", "Jett", 18, 6, 4, 8, 13, true),
    new ValorantMatch("Léa", "Reyan", 18, 6, 4, 8, 13, true),
});

var lol = DataSeries<LolMatch>.From(new[]

{
    new LolMatch("Noé", "Thresh", "Support", 7, 4, 8, 11, false),
    new LolMatch("Noé", "Thresh", "Support", 7, 4, 8, 11, false),
    new LolMatch("Noé", "Thresh", "Support", 7, 4, 8, 11, false),
});


Console.WriteLine($"Valorant match: {valorant.Count}");
Console.WriteLine($"LoL match: {lol.Count}");
