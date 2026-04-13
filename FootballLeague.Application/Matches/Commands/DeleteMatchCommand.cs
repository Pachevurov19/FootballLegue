using MediatR;
using System;

namespace FootballLeague.Application.Matches.Commands
{
    public class DeleteMatchCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
