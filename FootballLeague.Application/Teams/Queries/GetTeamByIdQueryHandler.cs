using FootballLeague.Application.Common.Exceptions;
using FootballLeague.Application.Teams.Dtos;
using FootballLeague.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static FootballLeague.Application.Constants.GlobalConstants;

namespace FootballLeague.Application.Teams.Queries
{
    public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, TeamDto>
    {
        private readonly AppDbContext _context;

        public GetTeamByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TeamDto> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (team == null)
                throw new BusinessException(teamNotFound);

            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name
            };
        }
    }
}
