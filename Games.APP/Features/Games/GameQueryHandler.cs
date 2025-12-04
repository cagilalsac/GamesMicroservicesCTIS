using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using Games.APP.Features.Publishers;
using Games.APP.Features.Tags;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Games.APP.Features.Games
{
    public class GameQueryRequest : Request, IRequest<IQueryable<GameQueryResponse>>
    {
    }

    public class GameQueryResponse : Response
    {
        public string Title { get; set; }

        public decimal Price { get; set; } // 1.2

        public DateTime? ReleaseDate { get; set; }

        public bool IsTopSeller { get; set; } // true, false

        public int PublisherId { get; set; } // 1

        public string PriceF { get; set; } // EN: $1.2, TR: 1,2₺

        public string ReleaseDateF { get; set; } // EN: 12/22/2025, TR: 22.12.2025

        public string IsTopSellerF { get; set; } // EN: Top Seller, Not Top Seller

        public string Publisher { get; set; } // EA, Valve

        public PublisherQueryResponse PublisherResponse { get; set; }

        public string Tags { get; set; } // Action, Adventure

        public List<TagQueryResponse> TagsQueryResponse { get; set; }

        public List<int> TagIds { get; set; }
    }

    public class GameQueryHandler : Service<Game>, IRequestHandler<GameQueryRequest, IQueryable<GameQueryResponse>>
    {
        public GameQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Game> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(g => g.GameTags).ThenInclude(gt => gt.Tag)
                .Include(g => g.Publisher).OrderByDescending(g => g.ReleaseDate).ThenBy(g => g.Title);
        }

        public Task<IQueryable<GameQueryResponse>> Handle(GameQueryRequest request, CancellationToken cancellationToken)
        {
            var query = Query().Select(g => new GameQueryResponse
            {
                Guid = g.Guid,
                Id = g.Id,
                IsTopSeller = g.IsTopSeller,
                Price = g.Price,
                PublisherId = g.PublisherId,
                Title = g.Title,
                ReleaseDate = g.ReleaseDate,

                IsTopSellerF = g.IsTopSeller ? "Top Seller" : "Not Top Seller",
                PriceF = g.Price.ToString("C2"),
                ReleaseDateF = g.ReleaseDate.HasValue ? g.ReleaseDate.Value.ToString("MM/dd/yyyy") : string.Empty,

                Publisher = g.Publisher.Name,
                PublisherResponse = new PublisherQueryResponse
                {
                    Guid = g.Publisher.Guid,
                    Id = g.Publisher.Id,
                    Name = g.Publisher.Name
                },

                Tags = string.Join(", ", g.GameTags.OrderBy(gt => gt.Tag.Name).Select(gt => gt.Tag.Name)),
                TagsQueryResponse = g.GameTags.OrderBy(gt => gt.Tag.Name).Select(gt => new TagQueryResponse
                {
                    Guid = gt.Tag.Guid,
                    Id = gt.Tag.Id,
                    Name = gt.Tag.Name
                }).ToList(),

                TagIds = g.TagIds
            });
            return Task.FromResult(query);
        }
    }
}
