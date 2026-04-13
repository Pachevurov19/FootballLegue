using MediatR;
using System;

namespace FootballLeague.Application.Teams.Commands
{
    public class CreateTeamCommand : IRequest<Guid>
    {
        public string Name { get; set; }
    }
}
