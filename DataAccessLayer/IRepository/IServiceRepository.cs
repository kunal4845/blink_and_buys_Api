using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetServices(int? serviceId);
        Task<bool> Delete(int serviceId, int loggedInUser);
        Task<int> Upsert(Service service, int loggedInUser);
        Task<List<BookedService>> GetBookedServices(int? bookedServiceId);
        Task<int> RejectService(int bookedServiceId, int loggedInUser);
        Task<int> RejectedByServiceProvider(int bookedServiceId, int loggedInUser);
        Task<int> ApprovedByServiceProvider(int bookedServiceId, int loggedInUser);
    }
}
