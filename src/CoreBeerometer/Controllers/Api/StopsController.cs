using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CoreBeerometer.Models;
using CoreBeerometer.Sevices;
using CoreBeerometer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreBeerometer.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private readonly IBeerRepository _repository;
        private readonly ILogger<StopsController> _logger;
        private readonly GeoCoordsService _service;

        public StopsController(IBeerRepository repository, 
            ILogger<StopsController> logger,
            GeoCoordsService service)
        {
            _repository = repository;
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName);
                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Stops: {ex}");
            }

            return BadRequest("Failed to get stops");
        }

        [HttpPost]
        public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);

                    var result = await  _service.GetGeoCoordsAsync(newStop.Name);
                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;

                        _repository.AddStop(tripName, newStop);

                        if (await _repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}",
                                Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                _logger.LogError("Failed to save new Stop: {0}", ex);
            }

            return BadRequest("Failed to save new stop");
        }
    }
}
