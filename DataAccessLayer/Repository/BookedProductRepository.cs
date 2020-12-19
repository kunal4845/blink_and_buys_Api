using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Grocery.Data;
using Database.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class BookedProductRepository : IBookedProductRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly ILogger<BookedProductRepository> _logger;
        public BookedProductRepository(BlinkandBuysContext dbContext, ILogger<BookedProductRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion

        public async Task<List<BookedProduct>> GetBookedProductAsync(int? bookedProductId, int loggedInUser)
        {
            try
            {
                var bookedProducts = new List<BookedProduct>();

                if (bookedProductId != null)
                {
                    var bookedProduct = await _dbContext.BookedProduct.FirstOrDefaultAsync(x => x.BookedProductId == bookedProductId && x.UserId == loggedInUser);
                    bookedProducts.Add(bookedProduct);
                }
                else
                {
                    bookedProducts = await _dbContext.BookedProduct.Where(x => x.UserId == loggedInUser).ToListAsync();
                }
                return bookedProducts;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<List<BookedService>> GetBookedServicesAsync(int? bookedServiceId, int loggedInUser)
        {
            try
            {
                var bookedServices = new List<BookedService>();

                if (bookedServiceId != null)
                {
                    var bookedService = await _dbContext.BookedService.FirstOrDefaultAsync(x => x.BookedServiceId == bookedServiceId 
                        && x.UserId == loggedInUser);
                    bookedServices.Add(bookedService);
                }
                else
                {
                    bookedServices = await _dbContext.BookedService.Where(x => x.UserId == loggedInUser).ToListAsync();
                }
                return bookedServices;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<bool> CancelOrder(int bookedProductId, int loggedInUser)
        {
            try
            {
                var bookedProduct = await _dbContext.BookedProduct.FirstOrDefaultAsync(x => x.BookedProductId == bookedProductId);
                if (bookedProduct != null)
                {
                    bookedProduct.IsCancelledByUser = true;
                    bookedProduct.ModifiedBy = loggedInUser;
                    bookedProduct.ModifiedDt = DateTime.Now;
                    _dbContext.BookedProduct.Update(bookedProduct);
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

        public async Task<bool> CancelService(int bookedServiceId, int loggedInUser)
        {
            try
            {
                var bookedService = await _dbContext.BookedService.FirstOrDefaultAsync(x => x.BookedServiceId == bookedServiceId);
                if (bookedService != null)
                {
                    bookedService.IsCancelledByUser = true;
                    bookedService.ModifiedBy = loggedInUser;
                    bookedService.ModifiedDt = DateTime.Now;
                    _dbContext.BookedService.Update(bookedService);
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
