using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> Get(int? categoryId);
        Task<bool> Delete(int categoryId, int loggedInUser);
        Task<int> Upsert(Category category, int loggedInUser);
        Task<List<SubCategory>> GetSubCategory(int? subCategoryId);
        Task<bool> DeleteSubCategory(int subCategoryId, int loggedInUser);
        Task<int> UpsertSubCategory(SubCategory subCategory, int loggedInUser);
        Task<List<SubCategory>> GetSubCategoryByService(int serviceId);
    }
}
