﻿using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {

        }

        public FootballBettingContext(DbContextOptions dbOptions) : base(dbOptions)
        {

        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=FootballBetting;Integrated Security=true");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(x => new { x.GameId, x.PlayerId });

            modelBuilder.Entity<Color>()
                .HasMany(x => x.PrimaryKitTeams)
                .WithOne(y => y.PrimaryKitColor)
                .HasForeignKey(z => z.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Color>()
                .HasMany(x => x.SecondaryKitTeams)
                .WithOne(y => y.SecondaryKitColor)
                .HasForeignKey(z => z.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany(x => x.HomeGames)
                .WithOne(y => y.HomeTeam)
                .HasForeignKey(z => z.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany(x => x.AwayGames)
                .WithOne(y => y.AwayTeam)
                .HasForeignKey(z => z.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}