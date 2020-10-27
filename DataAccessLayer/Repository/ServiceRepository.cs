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
    public class ServiceRepository : IServiceRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly ILogger<Service> _logger;
        public ServiceRepository(BlinkandBuysContext dbContext, ILogger<Service> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion
        public async Task<List<Service>> GetServices(int? serviceId)
        {
            try
            {
                _logger.LogError("Getting service list.");

                List<Service> services = new List<Service>();
                if (serviceId != null)
                {
                    var serviceList = await _dbContext.Service.ToListAsync();
                    services = serviceList.Where(x => x.Id == serviceId).ToList();
                }
                else
                {
                    services = await _dbContext.Service.ToListAsync();
                }

                return services;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<bool> Delete(int serviceId, int loggedInUser)
        {
            try
            {
                var service = await _dbContext.Service.FirstOrDefaultAsync(x => x.Id == serviceId);
                if (service != null)
                {
                    service.IsDeleted = true;
                    service.ModifiedDt = DateTime.Now;
                    service.ModifiedBy = loggedInUser;

                    _dbContext.Service.Update(service);
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

        public async Task<int> Upsert(Service service, int loggedInUser)
        {
            try
            {
                var obj = await _dbContext.Service.FirstOrDefaultAsync(x => x.Id == service.Id);
                if (obj != null)
                {
                    _logger.LogInformation("updating service record to database.");

                    obj.ModifiedBy = loggedInUser;
                    obj.ModifiedDt = DateTime.Now;
                    obj.ServiceIcon = service.ServiceIcon;
                    obj.ServiceName = service.ServiceName;
                    obj.Description = service.Description;
                    _dbContext.Service.Update(obj);
                }
                else
                {
                    _logger.LogInformation("inserting service record to database.");
                    service.IsActive = true;
                    service.IsDeleted = false;
                    service.CreatedBy = loggedInUser;
                    service.CreatedDt = DateTime.Now;
                    await _dbContext.Service.AddAsync(service);
                }
                await _dbContext.SaveChangesAsync();
                return service.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<List<BookedService>> GetBookedServices(int? bookedServiceId)
        {
            try
            {
                _logger.LogError("Getting booked services list.");

                List<BookedService> services = new List<BookedService>();
                if (bookedServiceId != null)
                {
                    var serviceList = await _dbContext.BookedService.ToListAsync();
                    services = serviceList.Where(x => x.BookedServiceId == bookedServiceId).ToList();
                }
                else
                {
                    services = await _dbContext.BookedService.ToListAsync();
                }

                return services;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
        
        public async Task<int> RejectService(int bookedServiceId, int loggedInUser)
        {
            try
            {
                var bookedService = await _dbContext.BookedService.FirstOrDefaultAsync(x => x.BookedServiceId == bookedServiceId);
                if (bookedService != null)
                {
                    _logger.LogInformation("updating BookedService record to database.");
                    bookedService.ModifiedBy = loggedInUser;
                    bookedService.ModifiedDt = DateTime.Now;
                    bookedService.IsRejected = true;
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
    }
}
