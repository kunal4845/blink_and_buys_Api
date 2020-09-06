using System;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IDealerRepository
    {
        Task<bool> VerifyDealer(int userId, int loggedInUser);
        Task<bool> DeleteDealer(int userId, int loggedInUser);
        Task<bool> BlockDealer(int userId, int loggedInUser);
    }
}
