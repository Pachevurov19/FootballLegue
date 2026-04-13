using FootballLeague.Application.Common.Exceptions;
using FootballLeague.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static FootballLeague.Application.Constants.GlobalConstants;

namespace FootballLeague.Application.Teams.Commands
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand>
    {
        private readonly AppDbContext _context;

        public UpdateTeamCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (team == null)
                throw new BusinessException(teamNotFound);

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BusinessException(teamNameIsRequired);

            var name = request.Name.Trim();

            var duplicate = await _context.Teams
                .AnyAsync(x => x.Name == name && x.Id != request.Id, cancellationToken);

            if (duplicate)
                throw new BusinessException(teamExists);

            team.Name = name;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
