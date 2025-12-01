using CORE.APP.Domain;

namespace Games.APP.Domain
{
    public class GameTag : Entity
    {
        public int GameId { get; set; }

        public Game Game { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}