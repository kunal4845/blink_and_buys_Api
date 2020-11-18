using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IPaymentRepository
    {
        Task<int> PostAsync(Payment payment, int loggedInUser);
    }
}
