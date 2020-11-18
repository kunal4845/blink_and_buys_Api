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
        private readonly ILogger<Product> _logger;
        public ProductRepository(BlinkandBuysContext dbContext, ILogger<Product> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion

        public async Task<int> Upsert(Product product, int loggedInUser)
        {
            try
            {
                var productObj = await _dbContext.Product.FirstOrDefaultAsync(x => x.Id == product.Id);
                if (productObj != null)
                {
                    _logger.LogInformation("updating product record to database.");
                    productObj.CommissionPercentage = product.CommissionPercentage;
                    productObj.Description = product.Description;
                    productObj.ModifiedBy = loggedInUser;
                    productObj.ModifiedDt = product.ModifiedDt;
                    productObj.Note = product.Note;
                    productObj.Price = product.Price;
                    productObj.ProductCategoryId = product.ProductCategoryId;
                    productObj.ProductName = product.ProductName;
                    productObj.ProductTitle = product.ProductTitle;
                    productObj.Size = product.Size;
                    productObj.Quantity = product.Quantity;
                    productObj.Specification = product.Specification;
                    productObj.IsActive = product.IsActive;
                    _dbContext.Product.Update(productObj);
                }
                else
                {
                    _logger.LogInformation("inserting product record to database.");
                    product.IsActive = true;
                    product.IsDeleted = false;
                    product.IsVerified = false;
                    product.CreatedBy = loggedInUser;
                    product.CreatedDt = DateTime.Now;
                    await _dbContext.Product.AddAsync(product);
                }
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
                    var product = await _dbContext.Product.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
                    products.Add(product);
                }
                else
                {
                    products = await _dbContext.Product.Where(x => !x.IsDeleted).ToListAsync();
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
                _logger.LogError("updating product to db.");
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

        public async Task<int> BlockProduct(Product product)
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
                    productImages = images.Where(x => x.ProductId == productId && !x.IsDeleted).ToList();
                }
                else
                {
                    productImages = _dbContext.ProductImage.Where(x => !x.IsDeleted).ToList();
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
                products = await _dbContext.Product.Where(x => !x.IsDeleted).ToListAsync();

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

        public async Task<UserCart> AddToCart(UserCart userCart)
        {
            try
            {
                _logger.LogError("adding cart to database.");
                var carts = await _dbContext.UserCart.ToListAsync();
                var cart = carts.Where(x => x.ProductId == userCart.ProductId && x.UserId == userCart.UserId && x.IsDeleted == false).FirstOrDefault();
                if (cart != null)
                {
                    cart.IsDeleted = false;
                    cart.ModifiedDt = DateTime.Now;
                    cart.ModifiedBy = userCart.UserId;
                    cart.Quantity = userCart.Quantity;
                    _dbContext.UserCart.Update(cart);
                }
                else
                {
                    userCart.IsDeleted = false;
                    userCart.CreatedDt = DateTime.Now;
                    userCart.CreatedBy = userCart.UserId;
                    await _dbContext.UserCart.AddAsync(userCart);
                }
                await _dbContext.SaveChangesAsync();
                return userCart;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<List<UserCart>> GetCart(int userId)
        {
            try
            {
                List<UserCart> products = await _dbContext.UserCart.ToListAsync();
                var res = products.Where(x => x.UserId == userId && x.IsDeleted == false).ToList();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteCart(int cartId)
        {
            try
            {
                _logger.LogError("adding cart to database.");
                var carts = await _dbContext.UserCart.ToListAsync();
                var cart = carts.Where(x => x.Id == cartId).FirstOrDefault();
                if (cart != null)
                {
                    cart.IsDeleted = true;
                    cart.ModifiedDt = DateTime.Now;
                    _dbContext.UserCart.Update(cart);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteProductImage(int productId)
        {
            try
            {
                var productImages = await _dbContext.ProductImage.Where(x => x.ProductId == productId).ToListAsync();
                if (productImages != null)
                {
                    _dbContext.ProductImage.RemoveRange(productImages);
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<UserCart> Checkout(List<UserCart> userCart)
        {
            try
            {

                return new UserCart();
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<BillingAddress> BillingAddress(BillingAddress billingAddress, int loggedInUser)
        {
            try
            {
                var address = await _dbContext.BillingAddress.FirstOrDefaultAsync(x => x.Id == billingAddress.Id);
                if (address != null)
                {
                    address.ModifiedDt = DateTime.Now;
                    address.ModifiedBy = loggedInUser;
                    address.Address = billingAddress.Address;
                    address.City = billingAddress.City;
                    address.FullName = billingAddress.FullName;
                    address.IsDeleted = billingAddress.IsDeleted;
                    address.LandMark = billingAddress.LandMark;
                    address.Mobile = billingAddress.Mobile;
                    address.State = billingAddress.State;
                    address.ZipCode = billingAddress.ZipCode;

                    _dbContext.BillingAddress.Update(address);
                }
                else
                {
                    billingAddress.ModifiedDt = DateTime.Now;
                    billingAddress.IsDeleted = false;
                    billingAddress.UserId = loggedInUser;
                    billingAddress.CreatedBy = loggedInUser;
                    billingAddress.CreatedDt = DateTime.Now;
                    await _dbContext.BillingAddress.AddAsync(billingAddress);
                }
                await _dbContext.SaveChangesAsync();
                return billingAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<BillingAddress> GetBillingAddress(int userId)
        {
            try
            {
                var address = await _dbContext.BillingAddress.FirstOrDefaultAsync(x => x.UserId == userId);
                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
    }
}
