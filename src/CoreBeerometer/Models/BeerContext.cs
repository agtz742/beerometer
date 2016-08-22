using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreBeerometer.Models
{
    public class BeerContext : DbContext
    {
        private readonly IConfigurationRoot _config;

        public BeerContext(IConfigurationRoot config, DbContextOptions options) : base(options)
        {
            _config = config;
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:BeerContextConnection"]);
        }
    }
}
