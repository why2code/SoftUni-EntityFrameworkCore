
using Theatre.Data.Models;
// ReSharper disable IdentifierTypo

namespace Theatre.Data
{
    using Microsoft.EntityFrameworkCore;

    // ReSharper disable once IdentifierTypo
    public class TheatreContext : DbContext
    {
        public TheatreContext() 
        {
        }

        public TheatreContext(DbContextOptions options)
        : base(options) 
        { 
        }

        public DbSet<Cast> Casts { get; set; } = null!;

        public DbSet<Play> Plays { get; set; } = null!;

        public DbSet<Models.Theatre> Theatres { get; set; } = null!;

        public DbSet<Ticket> Tickets { get; set; } = null!;

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>(e =>
            {
                e.HasKey(pk => new {pk.PlayId, pk.TheatreId});
            });
        }
    }
}