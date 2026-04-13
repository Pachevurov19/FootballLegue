using FootballLeague.Application.Common.Exceptions;
using FootballLeague.Application.Matches.Dtos;
using FootballLeague.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static FootballLeague.Application.Constants.GlobalConstants;

namespace FootballLeague.Application.Matches.Queries
{
    public class GetMatchByIdQueryHandler : IRequestHandler<GetMatchByIdQuery, MatchDto>
    {
        private readonly AppDbContext _context;

        public GetMatchByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MatchDto> Handle(GetMatchByIdQuery request, CancellationToken cancellationToken)
        {
            var match = await _context.Matches
                .AsNoTracking()
                .Include(x => x.HomeTeam)
                .Include(x => x.AwayTeam)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (match == null)
                throw new BusinessException(matchNotFound);

            return new MatchDto
            {
                Id = match.Id,
                HomeTeamId = match.HomeTeamId,
                HomeTeamName = match.HomeTeam.Name,
                AwayTeamId = match.AwayTeamId,
                AwayTeamName = match.AwayTeam.Name,
                HomeScore = match.HomeScore,
                AwayScore = match.AwayScore,
                PlayedAt = match.PlayedAt
            };
        }
    }
}
