﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Specification { get; set; }
        public string Size { get; set; }
        public int? Quantity { get; set; }
        public string Note { get; set; }
        public int? CommissionPercentage { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public int ModifiedBy { get; set; }
    }
}
