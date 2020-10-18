using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IUserRepository
    {
        Task<Account> SignUp(Account user);
        Task<Account> AuthenticateUser(Account user);
        Task<LoginToken> LoginToken(LoginToken token);
        Task<Account> CheckEmailExists(string email, int roleId);
        Task<Account> ResetPassword(Account account);
        Task<Account> GetUserById(int id);
        Task<List<Account>> GetUsers();
    }
}
