using MediatR;
using System;

namespace FootballLeague.Application.Matches.Commands
{
    public class UpdateMatchCommand : IRequest
    {
        public Guid Id { get; set; }

        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }

        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public DateTime PlayedAt { get; set; }
    }
}
