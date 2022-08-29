using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
            Bets = new HashSet<Bet>();
        }
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public ICollection<Bet> Bets { get; set; }

        //collection of bets

        //UserId, Username, Password, Email, Name, Balance
    }
}

