using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IProductRepository
    {
        //Task<List<ProductCategory>> GetProductCategory(int? id);
        Task<int> UploadProductImage(List<ProductImage> productImage, int productId);
        Task<int> Upsert(Product product, int loggedInUser);
        Task<List<Product>> GetProductsAsync(int? id);
        Task<int> VerifyProduct(Product product);
        Task<int> BlockProduct(Product product);
        Task<int> Delete(Product product);
        Task<List<ProductImage>> GetProductImages(int? productId);
        Task<List<Product>> GetRecommendedProducts();
        Task<UserCart> AddToCart(UserCart userCart);
        Task<List<UserCart>> GetCart(int userId);
        Task<bool> DeleteCart(int cartId);
        Task<bool> DeleteProductImage(int productId);
        Task<UserCart> Checkout(List<UserCart> userCart);
        Task<BillingAddress> BillingAddress(BillingAddress billingAddress, int loggedInUser);
        Task<BillingAddress> GetBillingAddress(int userId);
    }
}
