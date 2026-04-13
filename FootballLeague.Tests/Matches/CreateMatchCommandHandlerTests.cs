using FootballLeague.Application.Matches.Commands;
using FootballLeague.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static FootballLeague.Domain.Entities.Teams;

namespace FootballLeague.Tests.Matches
{
    public class CreateMatchCommandHandlerTests
    {
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Handle_Should_Return_Fail_When_HomeTeam_Does_Not_Exist()
        {
            using var context = CreateDbContext();

            context.Teams.Add(new Team
            {
                Id = Guid.NewGuid(),
                Name = "Arsenal",
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var handler = new CreateMatchCommandHandler(context);

            var command = new CreateMatchCommand
            {
                HomeTeamName = "Liverpool",
                AwayTeamName = "Arsenal",
                HomeScore = 2,
                AwayScore = 1,
                PlayedAt = DateTime.UtcNow
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Team 'Liverpool' does not exist.", result.Error);
        }

        [Fact]
        public async Task Handle_Should_Return_Fail_When_AwayTeam_Does_Not_Exist()
        {
            using var context = CreateDbContext();

            context.Teams.Add(new Team
            {
                Id = Guid.NewGuid(),
                Name = "Liverpool",
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var handler = new CreateMatchCommandHandler(context);

            var command = new CreateMatchCommand
            {
                HomeTeamName = "Liverpool",
                AwayTeamName = "Arsenal",
                HomeScore = 2,
                AwayScore = 1,
                PlayedAt = DateTime.UtcNow
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Team 'Arsenal' does not exist.", result.Error);
        }

        [Fact]
        public async Task Handle_Should_Return_Fail_When_Teams_Are_The_Same()
        {
            using var context = CreateDbContext();

            context.Teams.Add(new Team
            {
                Id = Guid.NewGuid(),
                Name = "Liverpool",
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var handler = new CreateMatchCommandHandler(context);

            var command = new CreateMatchCommand
            {
                HomeTeamName = "Liverpool",
                AwayTeamName = "Liverpool",
                HomeScore = 1,
                AwayScore = 0,
                PlayedAt = DateTime.UtcNow
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("A team cannot play against itself.", result.Error);
        }

        [Fact]
        public async Task Handle_Should_Create_Match_When_Data_Is_Valid()
        {
            using var context = CreateDbContext();

            var homeTeam = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Liverpool",
                CreatedAt = DateTime.UtcNow
            };

            var awayTeam = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Arsenal",
                CreatedAt = DateTime.UtcNow
            };

            context.Teams.AddRange(homeTeam, awayTeam);
            await context.SaveChangesAsync();

            var handler = new CreateMatchCommandHandler(context);

            var command = new CreateMatchCommand
            {
                HomeTeamName = "Liverpool",
                AwayTeamName = "Arsenal",
                HomeScore = 3,
                AwayScore = 2,
                PlayedAt = DateTime.UtcNow
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotEqual(Guid.Empty, result.Data);

            var savedMatch = await context.Matches.FirstOrDefaultAsync();

            Assert.NotNull(savedMatch);
            Assert.Equal(homeTeam.Id, savedMatch.HomeTeamId);
            Assert.Equal(awayTeam.Id, savedMatch.AwayTeamId);
            Assert.Equal(3, savedMatch.HomeScore);
            Assert.Equal(2, savedMatch.AwayScore);
        }
    }
}
