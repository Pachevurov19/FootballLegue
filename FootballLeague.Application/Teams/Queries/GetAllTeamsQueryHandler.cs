using FootballLeague.Application.Teams.Dtos;
using FootballLeague.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Application.Teams.Queries
{
    public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, List<TeamDto>>
    {
        private readonly AppDbContext _context;

        public GetAllTeamsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TeamDto>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Teams
                .AsNoTracking()
                .Select(x => new TeamDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}
