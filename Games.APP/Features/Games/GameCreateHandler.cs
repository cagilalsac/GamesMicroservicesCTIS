using CORE.APP.Models;
using CORE.APP.Services;
using Games.APP.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Games.APP.Features.Games
{
    public class GameCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(150)]
        public string Title { get; set; }

        public decimal Price { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsTopSeller { get; set; }

        public int PublisherId { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();
    }

    public class GameCreateHandler : Service<Game>, IRequestHandler<GameCreateRequest, CommandResponse>
    {
        public GameCreateHandler(DbContext db) : base(db)
        {
        }

        public Task<CommandResponse> Handle(GameCreateRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
