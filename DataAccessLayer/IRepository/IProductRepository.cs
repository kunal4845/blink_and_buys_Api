using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IProductRepository
    {
        Task<List<ProductCategory>> GetProductCategory(int? id);
        Task<int> UploadProductImage(List<ProductImage> productImage, int productId);
        Task<int> Upsert(Product product);
    }
}
