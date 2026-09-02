using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESport
{
    public class Cs2Match
    {
        // player	map	start_side	kills	deaths	assists	mvps	won
        public Cs2Match(string player, string map, string start_side, int kills, int deaths, int assists, int mvps, bool won)
        {
            Player = player;
            Map = map;
            StartSide = start_side;
            Kills = kills;
            Deaths = deaths;
            Assists = assists;
            Mvps = mvps;
            Won = won;
        }

        public string Player { get; }
        public string Map { get; }
        public string StartSide { get; }
        public int Kills { get; }
        public int Deaths { get; }
        public int Assists { get; }
        public int Mvps { get; }
        public bool Won { get; }
    }
}