using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int ProductId { get; set; }
        public bool IsPrimaryImage { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public int ModifiedBy { get; set; }
    }
}
