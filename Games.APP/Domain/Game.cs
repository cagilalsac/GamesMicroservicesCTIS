using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Games.APP.Domain
{
    public class Game : Entity
    {
        [Required, StringLength(150)]
        public string Title  { get; set; }

        public decimal Price { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsTopSeller { get; set; }

        public int PublisherId { get; set; }

        public Publisher Publisher { get; set; }

        public List<GameTag> GameTags { get; set; } = new List<GameTag>();
    }
}
