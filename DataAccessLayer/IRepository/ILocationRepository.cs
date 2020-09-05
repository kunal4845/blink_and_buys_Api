using System.Threading.Tasks;
using Database.Models;
using System.Collections.Generic;

namespace DataAccessLayer.IRepository {
    public interface ILocationRepository {
        Task<List<State>> GetStates();
        Task<List<City>> GetCities();
    }
}
