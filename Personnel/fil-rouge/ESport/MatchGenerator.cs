using System;
using System.Collections.Generic;
using System.Linq;
using DataSeries;

namespace ESport;

public static class MatchGenerator
{
    public static IEnumerable<DataPoint<Cs2Match>> GenerateCs2(string player, int count, int seed = 42)
    {
        Random rng = new Random(seed);
        string[] maps = new[] { "Dust2", "Mirage", "Inferno", "Nuke", "Ancient" };
        string[] sides = new[] { "CT", "T" };
        DateTime start = new DateTime(1970, 1, 1);

        return Enumerable.Range(1, count)
            .Select(i => new DataPoint<Cs2Match>(
                start.AddDays(i),
                new Cs2Match(
                    player,
                    maps[rng.Next(maps.Length)],
                    sides[rng.Next(sides.Length)], 
                    rng.Next(10, 28),              
                    rng.Next(6, 18),               
                    rng.Next(0, 8),                
                    rng.Next(0, 5),                
                    rng.Next(2) == 0      
                )
            ));
    }
}