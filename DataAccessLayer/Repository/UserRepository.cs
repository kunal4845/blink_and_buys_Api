using DataAccessLayer.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Grocery.Data;
using Database.Models;
using System.Collections.Generic;
using Core.Common;

namespace DataAccessLayer.Repository
{
    public class UserRepository : IUserRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly IMapper _mapper;
        public UserRepository(BlinkandBuysContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        #endregion

        public async Task<Account> SignUp(Account account)
        {
            try
            {
                account.IsActive = true;
                account.IsDeleted = false;
                account.IsVerified = false;
                account.CreatedDt = DateTime.Now;
                account.ModifiedDt = DateTime.Now;
                account.RoleId = (int)ERole.Dealer;

                await _dbContext.Account.AddAsync(account);
                await _dbContext.SaveChangesAsync();
                return account;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Account> AuthenticateUser(Account account)
        {
            try
            {
                Account user = null;
                user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Email.ToLower() == account.Email.ToLower());

                if (user != null)
                {

                    user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Email.ToLower() == account.Email.ToLower()
                                        && x.Password == account.Password);

                    return user;
                }
                return user;
            }
            catch (Exception ex)
            {
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
                throw ex;
            }
        }

        public async Task<Account> CheckEmailExists(string email)
        {
            try
            {
                Account user = null;
                user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
                return user;
            }
            catch (Exception ex)
            {
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
                throw ex;
            }
        }
    }
}
