using System;

namespace Core
{
    public class BookedProductModel
    {
        public int BookedProductId { get; set; }
        public int ProductId { get; set; }
        public int BillingAddressId { get; set; }
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsRejectedByAdmin { get; set; }
        public bool IsApprovedByAdmin { get; set; }
        public bool IsRejectedByDealer { get; set; }
        public bool IsApprovedByDealer { get; set; }
        public bool IsCancelledByUser { get; set; }
        public string DeliveryStatus { get; set; }
        public string PaymentMode { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public int? ModifiedBy { get; set; }
        public ProductModel Product { get; set; }
        public PaymentModel Payment { get; set; }
        public BillingAddressModel BillingAddress { get; set; }
        public AccountModel User { get; set; }
    }
}
