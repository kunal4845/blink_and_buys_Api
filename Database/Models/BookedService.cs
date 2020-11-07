using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class BookedService
    {
        [Key]
        public int BookedServiceId { get; set; }
        public int? ServiceProviderId { get; set; }
        public int ServiceId { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsRejectedByAdmin { get; set; }
        public bool IsApprovedByAdmin { get; set; }
        public bool IsRejectedByServiceProvider { get; set; }
        public bool IsApprovedByServiceProvider { get; set; }
        public bool IsCancelledByUser { get; set; }
        public DateTime CreatedDt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
