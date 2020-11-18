using System;

namespace Database.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string SubCategoryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public int ModifiedBy { get; set; }
    }
}
