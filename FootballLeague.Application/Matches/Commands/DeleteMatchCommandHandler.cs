using FootballLeague.Application.Common.Exceptions;
using FootballLeague.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static FootballLeague.Application.Constants.GlobalConstants;

namespace FootballLeague.Application.Matches.Commands
{
    public class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand>
    {
        private readonly AppDbContext _context;

        public DeleteMatchCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteMatchCommand request, CancellationToken cancellationToken)
        {
            var match = await _context.Matches
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (match == null)
                throw new BusinessException(matchNotFound);

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
