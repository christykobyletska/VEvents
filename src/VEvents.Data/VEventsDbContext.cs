using Blueshift.EntityFrameworkCore.MongoDB.Annotations;
using Blueshift.EntityFrameworkCore.MongoDB.Infrastructure;
using VEvents.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace VEvents.Data
{
    [MongoDatabase("VEvents")]
    public class VEventsDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public DbSet<EventLiker> EventLikers { get; set; }

        public VEventsDbContext()
            : this(new DbContextOptions<VEventsDbContext>())
        {
        }

        public VEventsDbContext(DbContextOptions<VEventsDbContext> VEventsDbContextOptions)
            : base(VEventsDbContextOptions)
        {
        }
    }
}
