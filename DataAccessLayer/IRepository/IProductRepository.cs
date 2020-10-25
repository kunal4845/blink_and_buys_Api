using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IProductRepository
    {
        //Task<List<ProductCategory>> GetProductCategory(int? id);
        Task<int> UploadProductImage(List<ProductImage> productImage, int productId);
        Task<int> Upsert(Product product);
        Task<List<Product>> GetProductsAsync(int? id);
        Task<int> VerifyProduct(Product product);
        Task<int> Delete(Product product);
        List<ProductImage> GetProductImages(int? productId);
        Task<List<Product>> GetRecommendedProducts();
        Task<UserCart> AddToCart(UserCart userCart);
        Task<List<UserCart>> GetCart(int userId);
        Task<bool> DeleteCart(int cartId);
    }
}
