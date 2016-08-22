using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoreBeerometer.Models;
using CoreBeerometer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreBeerometer.Controllers.Api
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private readonly IBeerRepository _repository;
        private readonly ILogger<TripsController> _logger;

        public TripsController(IBeerRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetAllTrips();
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all trips: {ex}");
                return BadRequest("Error Occured");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {
            if (ModelState.IsValid)
            {
                // Save to Database
                var newTrip = Mapper.Map<Trip>(theTrip);
                _repository.AddTrip(newTrip);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }
                else
                {
                    return BadRequest("Failed to save the trip");
                }
            }
            return BadRequest(ModelState);
        }
    }
}
