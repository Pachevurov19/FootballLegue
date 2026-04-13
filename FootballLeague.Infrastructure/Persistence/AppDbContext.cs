using FootballLeague.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static FootballLeague.Domain.Entities.Teams;

namespace FootballLeague.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(x => x.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.HomeScore).IsRequired();
                entity.Property(x => x.AwayScore).IsRequired();
                entity.Property(x => x.PlayedAt).IsRequired();

                entity.HasOne(x => x.HomeTeam)
                    .WithMany(x => x.HomeMatches)
                    .HasForeignKey(x => x.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AwayTeam)
                    .WithMany(x => x.AwayMatches)
                    .HasForeignKey(x => x.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}