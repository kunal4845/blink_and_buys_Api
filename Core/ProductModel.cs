using System;
using System.Collections.Generic;

namespace Core
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductTitle { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDt { get; set; }
        public List<ProductImageModel> ProductImages { get; set; }
    }

    public class ProductImageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string VideoPath { get; set; }
        public string MediaType { get; set; }
        public int ProductId { get; set; }
        public bool IsPrimaryImage { get; set; }
        public DateTime? CreatedDt { get; set; }
    }
}
