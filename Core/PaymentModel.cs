using System;

namespace Core
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PaymentType { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
    }
}
