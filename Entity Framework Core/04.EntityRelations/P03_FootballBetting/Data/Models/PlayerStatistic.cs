using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int ScoredGoals { get; set; }
        public int Assists { get; set; }
        public int MinutesPlayed { get; set; }

        //mapping table - composite key

        //GameId, PlayerId, ScoredGoals, Assists, MinutesPlayed
    }
}
