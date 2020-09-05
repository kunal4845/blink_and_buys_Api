using DataAccessLayer.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Grocery.Data;
using Database.Models;
using System.Collections.Generic;

namespace DataAccessLayer.Repository {
    public class LocationRepository : ILocationRepository {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly IMapper _mapper;
        public LocationRepository(BlinkandBuysContext dbContext, IMapper mapper) {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        #endregion

        public async Task<List<State>> GetStates() {
            try {
                var states = await _dbContext.State.ToListAsync();
                return states;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<List<City>> GetCities() {
            try {
                var cities = await _dbContext.City.ToListAsync();
                return cities;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
