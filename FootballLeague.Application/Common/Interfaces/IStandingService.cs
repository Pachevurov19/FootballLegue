using FootballLeague.Application.Standings.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Application.Common.Interfaces
{
    public interface IStandingService
    {
        Task<List<StandingDto>> CalculateAsync(CancellationToken cancellationToken = default);
    }
}
