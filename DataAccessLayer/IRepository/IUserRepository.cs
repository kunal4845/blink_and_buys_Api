using Database.Models;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository {
    public interface IUserRepository {
        Task<Account> SignUp(Account user);
        Task<Account> AuthenticateUser(Account user);
        Task<LoginToken> LoginToken(LoginToken token);
        Task<Account> CheckEmailExists(string email);
        Task<Account> ResetPassword(Account account);
        Task<Account> GetUserById(int id);
    }
}
