using FootballLeague.Application.Standings.Dtos;
using MediatR;
using System.Collections.Generic;

namespace FootballLeague.Application.Standings.Queries
{
    public class GetStandingsQuery : IRequest<List<StandingDto>>
    {
    }
}
