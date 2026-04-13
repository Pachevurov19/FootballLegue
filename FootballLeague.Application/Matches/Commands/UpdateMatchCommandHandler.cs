using FootballLeague.Application.Common.Exceptions;
using FootballLeague.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using static FootballLeague.Application.Constants.GlobalConstants;

namespace FootballLeague.Application.Matches.Commands
{
    public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand>
    {
        private readonly AppDbContext _context;

        public UpdateMatchCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
        {
            var match = await _context.Matches
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (match == null)
                throw new BusinessException(matchNotFound);

            if (string.IsNullOrWhiteSpace(request.HomeTeamName))
                throw new BusinessException(homeTeamNameIsRequired);

            if (string.IsNullOrWhiteSpace(request.AwayTeamName))
                throw new BusinessException(awayTeamNameIsRequired);

            if (request.HomeScore < 0 || request.AwayScore < 0)
                throw new BusinessException(scoresCannotBeNegative);

            var homeTeamName = request.HomeTeamName.Trim();
            var awayTeamName = request.AwayTeamName.Trim();

            if (string.Equals(homeTeamName, awayTeamName, StringComparison.OrdinalIgnoreCase))
                throw new BusinessException(teamCannotPlayAgainstItself);

            var homeTeam = await _context.Teams
                .FirstOrDefaultAsync(
                    x => x.Name.ToLower() == homeTeamName.ToLower(),
                    cancellationToken);

            if (homeTeam == null)
                throw new BusinessException($"Team '{homeTeamName}' does not exist.");

            var awayTeam = await _context.Teams
                .FirstOrDefaultAsync(
                    x => x.Name.ToLower() == awayTeamName.ToLower(),
                    cancellationToken);

            if (awayTeam == null)
                throw new BusinessException($"Team '{awayTeamName}' does not exist.");

            match.HomeTeamId = homeTeam.Id;
            match.AwayTeamId = awayTeam.Id;
            match.HomeScore = request.HomeScore;
            match.AwayScore = request.AwayScore;
            match.PlayedAt = request.PlayedAt;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
