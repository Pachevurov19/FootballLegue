using FootballLeague.Application.Teams.Dtos;
using MediatR;
using System.Collections.Generic;

namespace FootballLeague.Application.Teams.Queries
{
    public class GetAllTeamsQuery : IRequest<List<TeamDto>>
    {
    }
}
