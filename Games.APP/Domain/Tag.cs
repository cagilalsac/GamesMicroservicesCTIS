using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Games.APP.Domain
{
    public class Tag : Entity
    {
        [Required, StringLength(125)]
        public string Name { get; set; }

        public List<GameTag> GameTags { get; set; } = new List<GameTag>();
    }
}
