using FootballLeague.Application.Common.Models;
using FootballLeague.Domain.Entities;
using FootballLeague.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using static FootballLeague.Application.Constants.GlobalConstants;

namespace FootballLeague.Application.Matches.Commands
{
    public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, Result<Guid>>
    {
        private readonly AppDbContext _context;

        public CreateMatchCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.HomeTeamName))
                return Result<Guid>.Fail(homeTeamNameIsRequired);

            if (string.IsNullOrWhiteSpace(request.AwayTeamName))
                return Result<Guid>.Fail(awayTeamNameIsRequired);

            var homeName = request.HomeTeamName.Trim();
            var awayName = request.AwayTeamName.Trim();

            if (homeName.ToLower() == awayName.ToLower())
                return Result<Guid>.Fail(teamCannotPlayAgainstItself);

            var homeTeam = await _context.Teams
                .FirstOrDefaultAsync(x => x.Name.ToLower() == homeName.ToLower(), cancellationToken);

            if (homeTeam == null)
                return Result<Guid>.Fail($"Team '{homeName}' does not exist.");

            var awayTeam = await _context.Teams
                .FirstOrDefaultAsync(x => x.Name.ToLower() == awayName.ToLower(), cancellationToken);

            if (awayTeam == null)
                return Result<Guid>.Fail($"Team '{awayName}' does not exist.");

            if (request.HomeScore < 0 || request.AwayScore < 0)
                return Result<Guid>.Fail(scoresCannotBeNegative);

            var match = new Match
            {
                Id = Guid.NewGuid(),
                HomeTeamId = homeTeam.Id,
                AwayTeamId = awayTeam.Id,
                HomeScore = request.HomeScore,
                AwayScore = request.AwayScore,
                PlayedAt = request.PlayedAt
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Ok(match.Id);
        }
    }
}
