using FootballLeague.Application.Common.Models;
using MediatR;
using System;

namespace FootballLeague.Application.Matches.Commands
{
    public class CreateMatchCommand : IRequest<Result<Guid>>
    {
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }

        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public DateTime PlayedAt { get; set; }
    }
}
