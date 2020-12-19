using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IBookedProductRepository
    {
        Task<List<BookedProduct>> GetBookedProductAsync(int? bookedProductId, int loggedInUser);
        Task<bool> CancelOrder(int bookedProductId, int loggedInUser);
        Task<List<BookedService>> GetBookedServicesAsync(int? bookedServiceId, int loggedInUser);
        Task<bool> CancelService(int bookedServiceId, int loggedInUser);
    }
}
