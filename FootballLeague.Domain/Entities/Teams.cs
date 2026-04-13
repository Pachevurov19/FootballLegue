using System;
using System.Collections.Generic;

namespace FootballLeague.Domain.Entities
{
    public class Teams
    {
        public class Team
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public DateTime CreatedAt { get; set; }

            public ICollection<Match> HomeMatches { get; set; } = new List<Match>();
            public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
        }
    }
}
