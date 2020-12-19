using System;

namespace Core
{
    public class UserCartModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookedItemId { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDt { get; set; }
        public ProductModel Product { get; set; }
        public ServiceModel Service { get; set; }
    }
}
