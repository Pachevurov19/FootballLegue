using FootballLeague.Application.Matches.Dtos;
using MediatR;
using System;

namespace FootballLeague.Application.Matches.Queries
{
    public class GetMatchByIdQuery : IRequest<MatchDto>
    {
        public Guid Id { get; set; }
    }
}
