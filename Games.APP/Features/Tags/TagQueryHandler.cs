using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Games.APP.Features.Tags
{
    public class TagQueryRequest : Request, IRequest<IQueryable<TagQueryResponse>>
    {
    }

    public class TagQueryResponse : Response
    {
        public string Name { get; set; }
    }

    public class TagQueryHandler : Service<Tag>, IRequestHandler<TagQueryRequest, IQueryable<TagQueryResponse>>
    {
        public TagQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Tag> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).OrderBy(t => t.Name);
        }

        public Task<IQueryable<TagQueryResponse>> Handle(TagQueryRequest request, CancellationToken cancellationToken)
        {
            var query = Query().Select(t => new TagQueryResponse
            {
                Guid = t.Guid,
                Id = t.Id,
                Name = t.Name
            });
            return Task.FromResult(query);
        }
    }
}
