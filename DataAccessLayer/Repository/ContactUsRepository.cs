using DataAccessLayer.IRepository;
using System;
using System.Threading.Tasks;
using Grocery.Data;
using Database.Models;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repository
{
    public class ContactUsRepository : IContactUsRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly ILogger<ContactUsRepository> _logger;
        public ContactUsRepository(BlinkandBuysContext dbContext, ILogger<ContactUsRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion


        public async Task<int> PostAsync(ContactUs contact)
        {
            try
            {
                contact.CreatedDt = DateTime.Now;
                contact.ModifiedDt = DateTime.Now;
                await _dbContext.ContactUs.AddAsync(contact);
                await _dbContext.SaveChangesAsync();
                return contact.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
    }
}
