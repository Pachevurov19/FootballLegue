using FootballLeague.Application.Common.Exceptions;
using FootballLeague.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using static FootballLeague.Domain.Entities.Teams;
using static FootballLeague.Application.Constants.GlobalConstants;

namespace FootballLeague.Application.Teams.Commands
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Guid>
    {
        private readonly AppDbContext _context;

        public CreateTeamCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BusinessException(teamNameIsRequired);

            var teamName = request.Name.Trim();

            var exists = await _context.Teams
                .AnyAsync(x => x.Name.ToLower() == teamName.ToLower(), cancellationToken);

            if (exists)
                throw new BusinessException(teamExists);

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = teamName,
                CreatedAt = DateTime.UtcNow
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync(cancellationToken);

            return team.Id;
        }
    }
}
