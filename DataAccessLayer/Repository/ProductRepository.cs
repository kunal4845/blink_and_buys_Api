using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Grocery.Data;
using Database.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repository
{
    public class ProductRepository : IProductRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly ILogger<ProductCategory> _logger;
        public ProductRepository(BlinkandBuysContext dbContext, ILogger<ProductCategory> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion

        public async Task<List<ProductCategory>> GetProductCategory(int? id)
        {
            try
            {
                _logger.LogError("Getting product categories from db.");

                List<ProductCategory> categories = new List<ProductCategory>();
                if (id != null)
                {
                    var category = await _dbContext.ProductCategory.FirstOrDefaultAsync(x => x.Id == id);
                    categories.Add(category);
                }
                else
                {
                    categories = await _dbContext.ProductCategory.ToListAsync();
                }

                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

    }
}
