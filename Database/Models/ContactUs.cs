using System;

namespace Database.Models
{
    public class ContactUs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDt { get; set; }
        public int ModifiedBy { get; set; }
    }
}
