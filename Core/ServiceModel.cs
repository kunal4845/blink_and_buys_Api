using System;

namespace Core
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string ServiceIcon { get; set; }
        public decimal? Price { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime ModifiedDt { get; set; }
    }
}
