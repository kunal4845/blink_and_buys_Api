using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Grocery.Data;
using Database.Models;
using System.Collections.Generic;
using Core.Common;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repository
{
    public class UserRepository : IUserRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(BlinkandBuysContext dbContext, ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion

        public async Task<Account> SignUp(Account account)
        {
            try
            {
                if (account.RoleId == (int)ERole.Admin)
                {
                    account.IsAccountVerified = null;
                    account.IsNumberVerified = null;
                }

                if (account.RoleId == (int)ERole.Customer)
                {
                    account.IsAccountVerified = null;
                    account.IsNumberVerified = null;
                }

                if (account.RoleId == (int)ERole.Dealer)
                {
                    account.IsAccountVerified = false;
                    account.IsNumberVerified = false;
                }

                if (account.RoleId == (int)ERole.ServiceProvider)
                {
                    account.IsAccountVerified = false;
                    account.IsNumberVerified = null;
                }

                account.IsActive = true;
                account.IsDeleted = false;
                account.CreatedDt = DateTime.Now;
                account.ModifiedDt = DateTime.Now;
                account.RoleId = account.RoleId;
                await _dbContext.Account.AddAsync(account);
                await _dbContext.SaveChangesAsync();
                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<Account> AuthenticateUser(Account account)
        {
            try
            {
                Account user = null;
                user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Email.ToLower() == account.Email.ToLower() && x.RoleId == account.RoleId);
                if (user != null)
                {
                    user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Email.ToLower() == account.Email.ToLower()
                                        && x.Password == account.Password && x.RoleId == account.RoleId);
                    return user;
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<LoginToken> LoginToken(LoginToken token)
        {
            try
            {
                await _dbContext.LoginToken.AddAsync(token);
                await _dbContext.SaveChangesAsync();
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<Account> CheckEmailExists(string email, int roleId)
        {
            try
            {
                Account user = null;
                user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.RoleId == roleId);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<Account> ResetPassword(Account account)
        {
            try
            {
                account.ModifiedDt = DateTime.Now;
                _dbContext.Account.Update(account);
                await _dbContext.SaveChangesAsync();
                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<Account> GetUserById(int id)
        {
            try
            {
                Account user = null;
                user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<List<Account>> GetUsers()
        {
            try
            {
                var users = await _dbContext.Account.ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
    }
}
