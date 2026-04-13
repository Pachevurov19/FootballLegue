using FootballLeague.Application.Teams.Dtos;
using MediatR;
using System;

namespace FootballLeague.Application.Teams.Queries
{
    public class GetTeamByIdQuery : IRequest<TeamDto>
    {
        public Guid Id { get; set; }
    }
}
