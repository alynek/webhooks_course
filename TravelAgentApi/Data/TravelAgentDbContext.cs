using Microsoft.EntityFrameworkCore;
using TravelAgentApi.Models;

namespace TravelAgentApi.Data
{
    public class TravelAgentDbContext : DbContext
    {
        public TravelAgentDbContext(DbContextOptions<TravelAgentDbContext> options) : base(options){}

        public DbSet<WebhookSecret> WebhoookSecrets { get; set; }
    }
}