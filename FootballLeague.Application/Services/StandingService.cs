using FootballLeague.Application.Common.Interfaces;
using FootballLeague.Application.Standings.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FootballLeague.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Application.Services
{
    public class StandingService : IStandingService
    {
        private readonly AppDbContext _context;

        public StandingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StandingDto>> CalculateAsync(CancellationToken cancellationToken = default)
        {
            var teams = await _context.Teams
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var matches = await _context.Matches
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var standings = teams.ToDictionary(
                t => t.Id,
                t => new StandingDto
                {
                    TeamId = t.Id,
                    TeamName = t.Name
                });

            foreach (var match in matches)
            {
                var home = standings[match.HomeTeamId];
                var away = standings[match.AwayTeamId];

                home.Played++;
                away.Played++;

                home.GoalsFor += match.HomeScore;
                home.GoalsAgainst += match.AwayScore;

                away.GoalsFor += match.AwayScore;
                away.GoalsAgainst += match.HomeScore;

                if (match.HomeScore > match.AwayScore)
                {
                    home.Wins++;
                    home.Points += 3;
                    away.Losses++;
                }
                else if (match.HomeScore < match.AwayScore)
                {
                    away.Wins++;
                    away.Points += 3;
                    home.Losses++;
                }
                else
                {
                    home.Draws++;
                    away.Draws++;
                    home.Points++;
                    away.Points++;
                }
            }

            return standings.Values
                .OrderByDescending(x => x.Points)
                .ThenByDescending(x => x.GoalDifference)
                .ThenByDescending(x => x.GoalsFor)
                .ThenBy(x => x.TeamName)
                .ToList();
        }
    }
}
