using System.Collections.Generic;

namespace CoreBeerometer.Models
{
    public interface IBeerRepository
    {
        IEnumerable<Trip> GetAllTrips();
    }
}