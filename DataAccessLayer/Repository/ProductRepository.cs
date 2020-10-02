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
        public async Task<int> Upsert(Product product)
        {
            try
            {
                _logger.LogInformation("inserting product record to database.");
                product.IsActive = true;
                product.IsDeleted = false;
                product.IsVerified = false;
                product.CreatedBy = 1;
                product.CreatedDt = DateTime.Now;

                await _dbContext.Product.AddAsync(product);
                await _dbContext.SaveChangesAsync();
                return product.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
        public async Task<int> UploadProductImage(List<ProductImage> productImage, int productId)
        {
            try
            {
                _logger.LogInformation("inserting product images to database.");

                await _dbContext.ProductImage.AddRangeAsync(productImage);
                await _dbContext.SaveChangesAsync();
                return productId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
        public async Task<List<Product>> GetProductsAsync(int? id)
        {
            try
            {
                _logger.LogError("Getting product list from db.");

                List<Product> products = new List<Product>();
                if (id != null)
                {
                    var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.Id == id);
                    products.Add(product);
                }
                else
                {
                    products = await _dbContext.Product.ToListAsync();
                }

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
        public async Task<int> VerifyProduct(Product product)
        {
            try
            {
                _logger.LogError("Getting product list from db.");
                _dbContext.Product.Update(product);
                await _dbContext.SaveChangesAsync();
                return product.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
        public async Task<int> Delete(Product product)
        {
            try
            {
                _logger.LogError("Getting product list from db.");
                _dbContext.Product.Update(product);
                await _dbContext.SaveChangesAsync();
                return product.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public List<ProductImage> GetProductImages(int? productId)
        {
            try
            {
                _logger.LogError("Getting product images from db.");

                List<ProductImage> productImages = new List<ProductImage>();
                if (productId != null)
                {
                    var images = _dbContext.ProductImage.ToList();
                    productImages = images.Where(x => x.ProductId == productId).ToList();
                }
                else
                {
                    productImages = _dbContext.ProductImage.ToList();
                }

                return productImages;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<List<Product>> GetRecommendedProducts()
        {
            try
            {
                _logger.LogError("Getting product list from db.");
                List<Product> products = new List<Product>();
                products = await _dbContext.Product.ToListAsync();

                Random rand = new Random();
                int toSkip = rand.Next(1, products.Count);
                return products.OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(10).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }



    }
}
