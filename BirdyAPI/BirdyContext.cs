﻿using BirdyAPI.DataBaseModels;
using Microsoft.EntityFrameworkCore;

namespace BirdyAPI
{
    public class BirdyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public BirdyContext(DbContextOptions<BirdyContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friends>().HasKey(k => new {k.FirstUserID, k.SecondUserID});
        }
    }
}
