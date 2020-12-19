using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Grocery.Data;
using Database.Models;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class ServiceProviderRepository : IServiceProviderRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly ILogger<Service> _logger;
        public ServiceProviderRepository(BlinkandBuysContext dbContext, ILogger<Service> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion

        public async Task<int> AssignServiceProvider(int? serviceProviderId, int bookedServiceId, int loggedInUser)
        {
            try
            {
                var bookedService = await _dbContext.BookedService.FirstOrDefaultAsync(x => x.BookedServiceId == bookedServiceId);
                if (bookedService != null)
                {
                    _logger.LogInformation("updating BookedService record to database.");
                    bookedService.ModifiedBy = loggedInUser;
                    bookedService.ModifiedDt = DateTime.Now;
                    bookedService.ServiceProviderId = serviceProviderId;
                    bookedService.IsRejectedByAdmin = false;
                    bookedService.IsApprovedByAdmin = true;
                    _dbContext.BookedService.Update(bookedService);
                }

                await _dbContext.SaveChangesAsync();
                return bookedServiceId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<ServiceProviderAvailability> GetserviceProviderAvailability(int serviceProviderId)
        {
            try
            {
                _logger.LogError("Getting ServiceProviderAvailability list.");
                var serviceProviderAvailabilities = await _dbContext.ServiceProviderAvailability.ToListAsync();
                var serviceProviderAvailability = serviceProviderAvailabilities.Where(x => x.ServiceProviderId == serviceProviderId).FirstOrDefault();
                return serviceProviderAvailability;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<int> SetServiceProviderAvailability(ServiceProviderAvailability serviceProvider, int loggedInUser)
        {
            try
            {
                var serviceProviderAvailability = await _dbContext.ServiceProviderAvailability.FirstOrDefaultAsync(x => x.Id == serviceProvider.Id);
                if (serviceProviderAvailability != null)
                {
                    _logger.LogInformation("updating ServiceProviderAvailability record to database.");
                    serviceProviderAvailability.ModifiedBy = loggedInUser;
                    serviceProviderAvailability.ModifiedDt = DateTime.Now;
                    serviceProviderAvailability.Days = serviceProvider.Days;
                    serviceProviderAvailability.IsActive = serviceProvider.IsActive;
                    _dbContext.ServiceProviderAvailability.Update(serviceProviderAvailability);
                }
                else
                {
                    serviceProvider.CreatedBy = loggedInUser;
                    serviceProvider.CreatedDt = DateTime.Now;
                    await _dbContext.ServiceProviderAvailability.AddAsync(serviceProvider);
                }

                await _dbContext.SaveChangesAsync();
                return serviceProvider.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }


        public async Task<bool> VerifyServiceProvider(int userId, int loggedInUser)
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

        public async Task<bool> DeleteServiceProvider(int userId, int loggedInUser)
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

        public async Task<bool> BlockServiceProvider(int userId, int loggedInUser)
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

        public async Task<int> UpdateStatus(BookedService bookedService, string selectedPaymentStatus, int paymentId, int loggedInUser)
        {
            try
            {
                var payment = await _dbContext.Payment.FirstOrDefaultAsync(x => x.Id == paymentId);
                if (payment != null)
                {
                    payment.PaymentStatus = selectedPaymentStatus;
                    payment.ModifiedBy = loggedInUser;
                    payment.ModifiedDt = DateTime.Now;
                    _dbContext.Payment.Update(payment);
                    await _dbContext.SaveChangesAsync();
                }

                var service = await _dbContext.BookedService.Where(x => x.BookedServiceId == bookedService.BookedServiceId).FirstOrDefaultAsync();
                if (service != null)
                {
                    service.DeliveryStatus = bookedService.DeliveryStatus;
                    service.ModifiedBy = loggedInUser;
                    service.ModifiedDt = DateTime.Now;
                    _dbContext.Payment.Update(payment);
                    await _dbContext.SaveChangesAsync();
                }
                return bookedService.BookedServiceId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
    }
}