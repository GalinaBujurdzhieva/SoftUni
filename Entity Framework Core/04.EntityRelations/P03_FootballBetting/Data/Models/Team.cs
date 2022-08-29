using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            HomeGames = new HashSet<Game>();
            AwayGames = new HashSet<Game>();
            Players = new HashSet<Player>();
        }
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }
        public string LogoUrl { get; set; }

        [Required]
        public string Initials { get; set; }
        public decimal Budget { get; set; }
        public int PrimaryKitColorId { get; set; }
        public Color PrimaryKitColor { get; set; }
        public int SecondaryKitColorId { get; set; }
        public Color SecondaryKitColor { get; set; }
        public int TownId { get; set; }
        public Town Town { get; set; }

        public ICollection<Game> HomeGames { get; set; }
        public ICollection<Game> AwayGames { get; set; }
        public ICollection<Player> Players { get; set; }


        //collection of players
        //collection of games

        //TeamId, Name, LogoUrl, Initials (JUV, LIV, ARS…), Budget, PrimaryKitColorId, SecondaryKitColorId, TownId
    }
}