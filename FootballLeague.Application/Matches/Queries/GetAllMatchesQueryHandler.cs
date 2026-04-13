using FootballLeague.Application.Matches.Dtos;
using FootballLeague.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Application.Matches.Queries
{
    public class GetAllMatchesQueryHandler : IRequestHandler<GetAllMatchesQuery, List<MatchDto>>
    {
        private readonly AppDbContext _context;

        public GetAllMatchesQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MatchDto>> Handle(GetAllMatchesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Matches
                .AsNoTracking()
                .Include(x => x.HomeTeam)
                .Include(x => x.AwayTeam)
                .OrderByDescending(x => x.PlayedAt)
                .Select(x => new MatchDto
                {
                    Id = x.Id,
                    HomeTeamId = x.HomeTeamId,
                    HomeTeamName = x.HomeTeam.Name,
                    AwayTeamId = x.AwayTeamId,
                    AwayTeamName = x.AwayTeam.Name,
                    HomeScore = x.HomeScore,
                    AwayScore = x.AwayScore,
                    PlayedAt = x.PlayedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
