using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string ContactNumber { get; set; }
        public int? ServiceSubCategoryId { get; set; }
        public int? ServiceId { get; set; }
        public string Qualification { get; set; }
        public string Gender { get; set; }
        public string FatherName { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public string Address { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
        public int? ProductCategoryId { get; set; }
        public string IsGstAvailable { get; set; }
        public string GstNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string IfscCode { get; set; }
        public bool? IsNumberVerified { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool? IsAccountVerified { get; set; }
        public string IdProofPath { get; set; }
        public string CancelledChequePath { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime ModifiedDt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
