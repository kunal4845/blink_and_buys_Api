using Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetServices(int? serviceId);
        Task<bool> Delete(int serviceId, int loggedInUser);
        Task<int> Upsert(Service service, int loggedInUser);
    }
}
