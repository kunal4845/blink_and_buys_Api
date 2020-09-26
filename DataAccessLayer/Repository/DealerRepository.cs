using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Grocery.Data;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repository
{
    public class DealerRepository : IDealerRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly ILogger<DealerRepository> _logger;
        public DealerRepository(BlinkandBuysContext dbContext, ILogger<DealerRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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
                _logger.LogError("Following exception has occurred: {0}", ex);
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
                _logger.LogError("Following exception has occurred: {0}", ex);
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
                    user.IsActive = false;
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
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
    }
}
