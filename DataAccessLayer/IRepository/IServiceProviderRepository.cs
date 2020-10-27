using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IServiceProviderRepository
    {
        Task<int> AssignServiceProvider(int? serviceProviderId, int bookedServiceId, int loggedInUser);
        Task<ServiceProviderAvailability> GetserviceProviderAvailability(int serviceProviderId);
        Task<int> SetServiceProviderAvailability(ServiceProviderAvailability serviceProvider, int loggedInUser);
    }
}
