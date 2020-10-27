using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class ServiceProviderAvailabilityModel
    {
        public int Id { get; set; }
        public int ServiceProviderId { get; set; }
        public string Timings { get; set; }
        public string Days { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime? ModifiedDt { get; set; }
    }
}
