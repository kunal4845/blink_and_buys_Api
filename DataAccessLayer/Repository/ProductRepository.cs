﻿using DataAccessLayer.IRepository;
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

        //public async Task<List<ProductCategory>> GetProductCategory(int? id)
        //{
        //    try
        //    {
        //        _logger.LogError("Getting product categories from db.");

        //        List<ProductCategory> categories = new List<ProductCategory>();
        //        if (id != null)
        //        {
        //            var category = await _dbContext.ProductCategory.FirstOrDefaultAsync(x => x.Id == id);
        //            categories.Add(category);
        //        }
        //        else
        //        {
        //            categories = await _dbContext.ProductCategory.ToListAsync();
        //        }

        //        return categories;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Following exception has occurred: {0}", ex);
        //        throw ex;
        //    }
        //}

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
                    cart.Quantity = cart.Quantity + 1;
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
                _logger.LogError("Getting cart values from db.");
                List<UserCart> products = new List<UserCart>();
                products = await _dbContext.UserCart.ToListAsync();
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
    }
}
