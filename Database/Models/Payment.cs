using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PaymentType { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public int ModifiedBy { get; set; }
    }
}
