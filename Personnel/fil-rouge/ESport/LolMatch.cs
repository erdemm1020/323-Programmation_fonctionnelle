using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESport
{
    public class LolMatch
    {
        public LolMatch(string player, string champion, string role, int deaths, int assists, int cs, int visionScore, bool won)
        {
            Player = player;
            Champion = champion;
            Role = role;
            Deaths = deaths;
            Assists = assists;
            CS = cs;
            VisionScore = visionScore;
            Won = won;
        }

        public string Player { get; }
        public string Champion { get; }
        public string Role { get; }
        public int Deaths { get; }
        public int Assists { get; }
        public int CS { get; }
        public int VisionScore { get; }
        public bool Won { get; }
    }
}