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
        Task<bool> BlockServiceProvider(int userId, int loggedInUser);
        Task<bool> DeleteServiceProvider(int userId, int loggedInUser);
        Task<bool> VerifyServiceProvider(int userId, int loggedInUser);
        Task<int> UpdateStatus(BookedService bookedService, string selectedPaymentStatus, int paymentId, int loggedInUser);
    }
}
