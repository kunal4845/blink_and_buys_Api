using DataAccessLayer.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Grocery.Data;

namespace DataAccessLayer.Repository
{
    public class DealerRepository : IDealerRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        public DealerRepository(BlinkandBuysContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
        }
        #endregion

        public async Task<bool> VerifyDealer(int userId, int loggedInUser)
        {
            try
            {
                var user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    user.IsAccountVerified = true;
                    user.ModifiedDt = DateTime.Now;
                    user.ModifiedBy = loggedInUser;

                    _dbContext.Account.Update(user);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteDealer(int userId, int loggedInUser)
        {
            try
            {
                var user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    user.IsDeleted = true;
                    user.ModifiedDt = DateTime.Now;
                    user.ModifiedBy = loggedInUser;

                    _dbContext.Account.Update(user);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> BlockDealer(int userId, int loggedInUser)
        {
            try
            {
                var user = await _dbContext.Account.FirstOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    user.IsActive = true;
                    user.ModifiedDt = DateTime.Now;
                    user.ModifiedBy = loggedInUser;

                    _dbContext.Account.Update(user);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
