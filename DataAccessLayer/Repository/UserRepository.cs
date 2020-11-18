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
                if (account.RoleId == (int)ERole.Admin || account.RoleId == (int)ERole.Customer)
                {
                    user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Email.ToLower() == account.Email.ToLower()
                        && x.RoleId == account.RoleId && x.IsActive && !x.IsDeleted);
                }
                else if (account.RoleId == (int)ERole.ServiceProvider)
                {
                    user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Email.ToLower() == account.Email.ToLower()
                        && x.RoleId == account.RoleId && x.IsActive && !x.IsDeleted && x.IsAccountVerified.Value);
                }
                else if (account.RoleId == (int)ERole.Dealer)
                {
                    user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Email.ToLower() == account.Email.ToLower()
                        && x.RoleId == account.RoleId && x.IsActive && !x.IsDeleted && x.IsAccountVerified.Value && x.IsNumberVerified.Value);
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
                user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
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

        public async Task<bool> UpdatePassword(int userId, string password, string confirmPassword)
        {
            try
            {
                var user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == userId);
                if (user.Password == password)
                {
                    user.Password = confirmPassword;
                    user.ModifiedDt = DateTime.Now;
                    _dbContext.Account.Update(user);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<bool> UpdateProfileImage(int userId, string profileImage)
        {
            try
            {
                var user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    user.ModifiedDt = DateTime.Now;
                    user.ModifiedBy = userId;
                    user.Image = profileImage;
                    _dbContext.Account.Update(user);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
    }
}
