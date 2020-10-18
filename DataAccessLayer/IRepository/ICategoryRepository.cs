using Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> Get(int? categoryId);
        Task<bool> Delete(int categoryId, int loggedInUser);
        Task<int> Upsert(Category category, int loggedInUser);
    }
}
