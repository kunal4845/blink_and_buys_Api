using System;

namespace Core
{
    public class BillingAddressModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string LandMark { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
    }
}
