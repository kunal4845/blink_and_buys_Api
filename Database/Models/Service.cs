using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string ServiceIcon { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public int ModifiedBy { get; set; }
    }
}
