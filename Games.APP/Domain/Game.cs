using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Games.APP.Domain
{
    public class Game : Entity
    {
        [Required, StringLength(150)]
        public string Title { get; set; }

        public decimal Price { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsTopSeller { get; set; }

        public int PublisherId { get; set; }

        public Publisher Publisher { get; set; }

        public List<GameTag> GameTags { get; set; } = new List<GameTag>();

        [NotMapped]
        public List<int> TagIds
        {
            get => GameTags.Select(gt => gt.TagId).ToList();
            set => GameTags = value.Select(v => new GameTag { TagId = v }).ToList();
        }
    }
}
