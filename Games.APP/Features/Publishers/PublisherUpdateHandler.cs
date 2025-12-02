using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Games.APP.Features.Publishers
{
    public class PublisherUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class PublisherUpdateHandler : Service<Publisher>, IRequestHandler<PublisherUpdateRequest, CommandResponse>
    {
        public PublisherUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(PublisherUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(p => p.Id != request.Id && p.Name == request.Name.Trim(), cancellationToken))
                return Error("Publisher with the same name exists!");
            var entity = await Query(false).SingleOrDefaultAsync(p => p.Id == request.Id);
            if (entity is null)
                return Error("Publisher not found!");
            entity.Name = request.Name.Trim();
            await Update(entity, cancellationToken);
            return Success("Publisher updated successfully.", entity.Id);
        }
    }
}
