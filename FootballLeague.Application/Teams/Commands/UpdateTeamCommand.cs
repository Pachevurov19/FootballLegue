using MediatR;
using System;

namespace FootballLeague.Application.Teams.Commands
{
    public class UpdateTeamCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
