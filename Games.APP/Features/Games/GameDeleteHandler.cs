using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Games.APP.Features.Games
{
    public class GameDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class GameDeleteHandler : Service<Game>, IRequestHandler<GameDeleteRequest, CommandResponse>
    {
        public GameDeleteHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Game> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(g => g.GameTags);
        }

        public async Task<CommandResponse> Handle(GameDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false).SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Game not found!");
            Delete(entity.GameTags);
            await Delete(entity, cancellationToken);
            return Success("Game deleted successfully.", entity.Id);
        }
    }
}
