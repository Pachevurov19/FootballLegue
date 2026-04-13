using FootballLeague.Application.Common.Exceptions;
using FootballLeague.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static FootballLeague.Application.Constants.GlobalConstants;

namespace FootballLeague.Application.Teams.Commands
{
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand>
    {
        private readonly AppDbContext _context;

        public DeleteTeamCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (team == null)
                throw new BusinessException(teamNotFound);

            var hasMatches = await _context.Matches
                .AnyAsync(x => x.HomeTeamId == request.Id || x.AwayTeamId == request.Id, cancellationToken);

            if (hasMatches)
                throw new BusinessException(cannotDeleteTeamWithPlayedMatches);

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
