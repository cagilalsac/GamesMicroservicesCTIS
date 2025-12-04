using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Games.APP.Features.Tags
{
    public class TagCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")] 
        [StringLength(125, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }
    }

    public class TagCreateHandler : Service<Tag>, IRequestHandler<TagCreateRequest, CommandResponse>
    {
        public TagCreateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TagCreateRequest request, CancellationToken cancellationToken)
        {
            if (await Query().AnyAsync(t => t.Name == request.Name.Trim(), cancellationToken))
                return Error("Tag with the same name exists!");
            var entity = new Tag
            {
                Name = request.Name.Trim()
            };
            await Create(entity, cancellationToken);
            return Success("Tag created successfully.", entity.Id);
        }
    }
}
