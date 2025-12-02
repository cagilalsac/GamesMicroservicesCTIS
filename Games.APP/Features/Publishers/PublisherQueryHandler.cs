using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Games.APP.Features.Publishers
{
    public class PublisherQueryRequest : Request, IRequest<IQueryable<PublisherQueryResponse>>
    {
    }

    public class PublisherQueryResponse : Response
    {
        public string Name { get; set; }
    }

    public class PublisherQueryHandler : Service<Publisher>, IRequestHandler<PublisherQueryRequest, IQueryable<PublisherQueryResponse>>
    {
        public PublisherQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Publisher> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).OrderBy(p => p.Name);
        }

        public Task<IQueryable<PublisherQueryResponse>> Handle(PublisherQueryRequest request, CancellationToken cancellationToken)
        {
            var query = Query().Select(p => new PublisherQueryResponse
            {
                Guid = p.Guid,
                Id = p.Id,
                Name = p.Name
            });
            return Task.FromResult(query);
        }
    }
}
