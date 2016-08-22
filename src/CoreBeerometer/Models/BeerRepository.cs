using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace CoreBeerometer.Models
{
    public class BeerRepository : IBeerRepository
    {
        private readonly BeerContext _context;
        private readonly ILogger<BeerRepository> _logger;

        public BeerRepository(BeerContext context, ILogger<BeerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting All Trtips from the Database");
            return _context.Trips.ToList();
        }
    }
}
