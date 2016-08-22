using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBeerometer.Models
{
    public interface IBeerRepository
    {
        IEnumerable<Trip> GetAllTrips();

        void AddTrip(Trip trip);
        void AddStop(string tripName, Stop newStop);

        Task<bool> SaveChangesAsync();
        Trip GetTripByName(string tripName);
    }
}