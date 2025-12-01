using Microsoft.EntityFrameworkCore;

namespace Games.APP.Domain
{
    public class GamesDb : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<GameTag> GameTags { get; set; }

        public GamesDb(DbContextOptions options) : base(options)
        {
        }
    }
}
