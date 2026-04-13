using MediatR;
using System;

namespace FootballLeague.Application.Teams.Commands
{
    public class DeleteTeamCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
