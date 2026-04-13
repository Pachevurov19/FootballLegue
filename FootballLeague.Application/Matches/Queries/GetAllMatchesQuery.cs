using FootballLeague.Application.Matches.Dtos;
using MediatR;
using System.Collections.Generic;

namespace FootballLeague.Application.Matches.Queries
{
    public class GetAllMatchesQuery : IRequest<List<MatchDto>>
    {
    }
}
