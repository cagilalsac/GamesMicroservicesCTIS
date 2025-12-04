using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Games.APP.Features.Tags
{
    public class TagUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(125, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }
    }

    public class TagUpdateHandler : Service<Tag>, IRequestHandler<TagUpdateRequest, CommandResponse>
    {
        public TagUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TagUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(t => t.Id != request.Id && t.Name == request.Name.Trim(), cancellationToken))
                return Error("Tag with the same name exists!");
            var entity = await Query(false).SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Tag not found!");
            entity.Name = request.Name.Trim();
            await Update(entity, cancellationToken);
            return Success("Tag updated successfully.", entity.Id);
        }
    }
}
