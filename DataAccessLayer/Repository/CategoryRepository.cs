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
    public class CategoryRepository : ICategoryRepository
    {
        #region"CONTEXT"
        private readonly BlinkandBuysContext _dbContext;
        private readonly ILogger<Category> _logger;
        public CategoryRepository(BlinkandBuysContext dbContext, ILogger<Category> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion

        public async Task<List<Category>> Get(int? categoryId)
        {
            try
            {
                _logger.LogError("Getting Category list.");

                List<Category> categories = new List<Category>();
                if (categoryId != null)
                {
                    var categoryList = await _dbContext.Category.ToListAsync();
                    categories = categoryList.Where(x => x.Id == categoryId).ToList();
                }
                else
                {
                    categories = await _dbContext.Category.ToListAsync();
                }

                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<bool> Delete(int categoryId, int loggedInUser)
        {
            try
            {
                var category = await _dbContext.Category.FirstOrDefaultAsync(x => x.Id == categoryId);
                if (category != null)
                {
                    category.IsDeleted = true;
                    category.ModifiedDt = DateTime.Now;
                    category.ModifiedBy = loggedInUser;

                    _dbContext.Category.Update(category);
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

        public async Task<int> Upsert(Category category, int loggedInUser)
        {
            try
            {
                var obj = await _dbContext.Category.FirstOrDefaultAsync(x => x.Id == category.Id);
                if (obj != null)
                {
                    _logger.LogInformation("updating category record to database.");

                    obj.ModifiedBy = loggedInUser;
                    obj.ModifiedDt = DateTime.Now;
                    obj.CategoryName = category.CategoryName;
                    obj.Description = category.Description;
                    _dbContext.Category.Update(obj);
                }
                else
                {
                    _logger.LogInformation("inserting category record to database.");
                    category.IsActive = true;
                    category.IsDeleted = false;
                    category.CreatedBy = loggedInUser;
                    category.CreatedDt = DateTime.Now;
                    await _dbContext.Category.AddAsync(category);
                }
                await _dbContext.SaveChangesAsync();
                return category.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<List<SubCategory>> GetSubCategory(int? subCategoryId)
        {
            try
            {
                _logger.LogError("Getting subCategory list.");
                List<SubCategory> categories = new List<SubCategory>();
                if (subCategoryId != null)
                {
                    var categoryList = await _dbContext.SubCategory.ToListAsync();
                    categories = categoryList.Where(x => x.Id == subCategoryId).ToList();
                }
                else
                {
                    categories = await _dbContext.SubCategory.ToListAsync();
                }

                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<List<SubCategory>> GetSubCategoryByService(int serviceId)
        {
            try
            {
                var subCategories = await _dbContext.SubCategory.Where(x => x.ServiceId == serviceId).ToListAsync();
                return subCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteSubCategory(int subCategoryId, int loggedInUser)
        {
            try
            {
                var subCategory = await _dbContext.SubCategory.FirstOrDefaultAsync(x => x.Id == subCategoryId);
                if (subCategory != null)
                {
                    subCategory.IsDeleted = true;
                    subCategory.ModifiedDt = DateTime.Now;
                    subCategory.ModifiedBy = loggedInUser;

                    _dbContext.SubCategory.Update(subCategory);
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

        public async Task<int> UpsertSubCategory(SubCategory subCategory, int loggedInUser)
        {
            try
            {
                var obj = await _dbContext.SubCategory.FirstOrDefaultAsync(x => x.Id == subCategory.Id);
                if (obj != null)
                {
                    _logger.LogInformation("updating category record to database.");

                    obj.ModifiedBy = loggedInUser;
                    obj.ModifiedDt = DateTime.Now;
                    obj.SubCategoryName = subCategory.SubCategoryName;
                    obj.Description = subCategory.Description;
                    obj.ServiceId = subCategory.ServiceId;
                    _dbContext.SubCategory.Update(obj);
                }
                else
                {
                    _logger.LogInformation("inserting category record to database.");
                    subCategory.IsActive = true;
                    subCategory.IsDeleted = false;
                    subCategory.CreatedBy = loggedInUser;
                    subCategory.CreatedDt = DateTime.Now;
                    await _dbContext.SubCategory.AddAsync(subCategory);
                }
                await _dbContext.SaveChangesAsync();
                return subCategory.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Following exception has occurred: {0}", ex);
                throw ex;
            }
        }
    }
}
