using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Games.APP.Features.Publishers
{
    public class PublisherDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class PublisherDeleteHandler : Service<Publisher>, IRequestHandler<PublisherDeleteRequest, CommandResponse>
    {
        public PublisherDeleteHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(PublisherDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await Query(false).SingleOrDefaultAsync(p => p.Id == request.Id);
            if (entity is null)
                return Error("Publisher not found!");
            await Delete(entity, cancellationToken);
            return Success("Publisher deleted successfully.", entity.Id);
        }
    }
}
