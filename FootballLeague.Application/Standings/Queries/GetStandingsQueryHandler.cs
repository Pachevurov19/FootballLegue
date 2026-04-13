using FootballLeague.Application.Common.Interfaces;
using FootballLeague.Application.Standings.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Application.Standings.Queries
{
    public class GetStandingsQueryHandler : IRequestHandler<GetStandingsQuery, List<StandingDto>>
    {
        private readonly IStandingService _standingService;

        public GetStandingsQueryHandler(IStandingService standingService)
        {
            _standingService = standingService;
        }

        public Task<List<StandingDto>> Handle(GetStandingsQuery request, CancellationToken cancellationToken)
        {
            return _standingService.CalculateAsync(cancellationToken);
        }
    }
}
