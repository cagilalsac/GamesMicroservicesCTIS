using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Games.APP.Features.Tags
{
    public class TagDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class TagDeleteHandler : Service<Tag>, IRequestHandler<TagDeleteRequest, CommandResponse>
    {
        public TagDeleteHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Tag> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(t => t.GameTags);
        }

        public async Task<CommandResponse> Handle(TagDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false).SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Tag not found!");
            Delete(entity.GameTags);
            await Delete(entity, cancellationToken);
            return Success("Tag deleted successfully.", entity.Id);
        }
    }
}
