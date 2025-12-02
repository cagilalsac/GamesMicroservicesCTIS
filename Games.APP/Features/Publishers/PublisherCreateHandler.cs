using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Games.APP.Features.Publishers
{
    public class PublisherCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class PublisherCreateHandler : Service<Publisher>, IRequestHandler<PublisherCreateRequest, CommandResponse>
    {
        public PublisherCreateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(PublisherCreateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(p => p.Name == request.Name.Trim(), cancellationToken))
                return Error("Publisher with the same name exists!");
            var entity = new Publisher
            {
                Name = request.Name.Trim()
            };
            await Create(entity, cancellationToken);
            return Success("Publisher created successfully.", entity.Id);
        }
    }
}
