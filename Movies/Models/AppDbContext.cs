using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<UserMovie> UserMovie { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMovie>()
    .HasKey(t => new { t.UserID, t.MovieId });

            modelBuilder.Entity<UserMovie>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserMovies)
                .HasForeignKey(pt => pt.UserID);

            modelBuilder.Entity<UserMovie>()
                .HasOne(pt => pt.Movie)
                .WithMany(t => t.UserMovies)
                .HasForeignKey(pt => pt.MovieId);
            base.OnModelCreating(modelBuilder);

        }
    }
}
