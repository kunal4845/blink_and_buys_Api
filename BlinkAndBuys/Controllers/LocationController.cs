using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using DataAccessLayer.IRepository;
using Database.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlinkAndBuys.Controllers {
    [Route("api/[controller]")]
    [EnableCors("CORS")]
    [ApiController]
    public class LocationController : ControllerBase {
        #region Initiate
        private ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationController(ILocationRepository locationRepository, IMapper mapper) {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }
        #endregion

        [HttpGet]
        [Route("cities")]
        public async Task<IActionResult> GetCities() {
            try {
                var cities = await _locationRepository.GetCities();
                return Ok(_mapper.Map<List<City>, List<CityModel>>(cities));
            }
            catch (Exception ex) {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("states")]
        public async Task<IActionResult> GetStates() {
            try {
                var states = await _locationRepository.GetStates();
                return Ok(_mapper.Map<List<State>, List<StateModel>>(states));
            }
            catch (Exception ex) {
                return BadRequest();
            }
        }
    }
}