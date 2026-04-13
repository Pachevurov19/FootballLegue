using FootballLeague.Application.Services;
using FootballLeague.Domain.Entities;
using FootballLeague.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static FootballLeague.Domain.Entities.Teams;

namespace FootballLeague.Tests.Services
{
    public class StandingServiceTests
    {
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CalculateAsync_Should_Give_3_Points_For_Win()
        {
            using var context = CreateDbContext();

            var teamA = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Liverpool",
                CreatedAt = DateTime.UtcNow
            };

            var teamB = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Arsenal",
                CreatedAt = DateTime.UtcNow
            };

            context.Teams.AddRange(teamA, teamB);

            context.Matches.Add(new Match
            {
                Id = Guid.NewGuid(),
                HomeTeamId = teamA.Id,
                AwayTeamId = teamB.Id,
                HomeScore = 2,
                AwayScore = 1,
                PlayedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var service = new StandingService(context);
            var standings = await service.CalculateAsync();

            var liverpool = standings.First(x => x.TeamId == teamA.Id);
            var arsenal = standings.First(x => x.TeamId == teamB.Id);

            Assert.Equal(3, liverpool.Points);
            Assert.Equal(0, arsenal.Points);
            Assert.Equal(1, liverpool.Wins);
            Assert.Equal(1, arsenal.Losses);
        }

        [Fact]
        public async Task CalculateAsync_Should_Give_1_Point_To_Both_Teams_For_Draw()
        {
            using var context = CreateDbContext();

            var teamA = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Chelsea",
                CreatedAt = DateTime.UtcNow
            };

            var teamB = new Team
            {
                Id = Guid.NewGuid(),
                Name = "United",
                CreatedAt = DateTime.UtcNow
            };

            context.Teams.AddRange(teamA, teamB);

            context.Matches.Add(new Match
            {
                Id = Guid.NewGuid(),
                HomeTeamId = teamA.Id,
                AwayTeamId = teamB.Id,
                HomeScore = 1,
                AwayScore = 1,
                PlayedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var service = new StandingService(context);
            var standings = await service.CalculateAsync();

            var chelsea = standings.First(x => x.TeamId == teamA.Id);
            var united = standings.First(x => x.TeamId == teamB.Id);

            Assert.Equal(1, chelsea.Points);
            Assert.Equal(1, united.Points);
            Assert.Equal(1, chelsea.Draws);
            Assert.Equal(1, united.Draws);
        }

        [Fact]
        public async Task CalculateAsync_Should_Order_By_Points_Then_GoalDifference()
        {
            using var context = CreateDbContext();

            var teamA = new Team { Id = Guid.NewGuid(), Name = "A", CreatedAt = DateTime.UtcNow };
            var teamB = new Team { Id = Guid.NewGuid(), Name = "B", CreatedAt = DateTime.UtcNow };
            var teamC = new Team { Id = Guid.NewGuid(), Name = "C", CreatedAt = DateTime.UtcNow };

            context.Teams.AddRange(teamA, teamB, teamC);

            context.Matches.AddRange(
                new Match
                {
                    Id = Guid.NewGuid(),
                    HomeTeamId = teamA.Id,
                    AwayTeamId = teamB.Id,
                    HomeScore = 3,
                    AwayScore = 0,
                    PlayedAt = DateTime.UtcNow
                },
                new Match
                {
                    Id = Guid.NewGuid(),
                    HomeTeamId = teamC.Id,
                    AwayTeamId = teamB.Id,
                    HomeScore = 1,
                    AwayScore = 0,
                    PlayedAt = DateTime.UtcNow.AddMinutes(1)
                }
            );

            await context.SaveChangesAsync();

            var service = new StandingService(context);
            var standings = await service.CalculateAsync();

            Assert.Equal(teamA.Id, standings[0].TeamId);
            Assert.Equal(teamC.Id, standings[1].TeamId);
            Assert.Equal(teamB.Id, standings[2].TeamId);

            Assert.Equal(3, standings[0].Points);
            Assert.Equal(3, standings[1].Points);
            Assert.True(standings[0].GoalDifference > standings[1].GoalDifference);
        }
    }
}