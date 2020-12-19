using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class UserCart
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookedItemId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public int ModifiedBy { get; set; }
    }
}
