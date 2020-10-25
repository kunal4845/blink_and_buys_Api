using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class BookedServiceModel
    {
        public int BookedServiceId { get; set; }
        public int? ServiceProviderId { get; set; }
        public int ServiceId { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsRejected { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDt { get; set; }
    }
}
