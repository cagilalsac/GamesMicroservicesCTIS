using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Games.APP.Domain
{
    public class Publisher : Entity
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        public List<Game> Games { get; set; } = new List<Game>();
    }
}