using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Games.APP.Features.Games
{
    public class GameUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(150)]
        public string Title { get; set; }

        public decimal Price { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsTopSeller { get; set; }

        public int PublisherId { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();
    }

    public class GameUpdateHandler : Service<Game>, IRequestHandler<GameUpdateRequest, CommandResponse>
    {
        public GameUpdateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Game> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(g => g.GameTags);
        }

        public async Task<CommandResponse> Handle(GameUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(g => g.Id != request.Id && g.Title == request.Title.Trim(), cancellationToken))
                return Error("Game with same title exists!");
            var entity = await Query(false).SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Game not found!");
            Delete(entity.GameTags);
            entity.IsTopSeller = request.IsTopSeller;
            entity.Price = request.Price;
            entity.PublisherId = request.PublisherId;
            entity.ReleaseDate = request.ReleaseDate;
            entity.TagIds = request.TagIds;
            entity.Title = request.Title.Trim();
            await Update(entity, cancellationToken);
            return Success("Game updated successfully.", entity.Id);
        }
    }
}
