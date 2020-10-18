using System;
using System.IO;

namespace Core
{
    public class AccountModel
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
        public string ProductCategoryId { get; set; }
        public string IsGstAvailable { get; set; }
        public string GstNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string IfscCode { get; set; }
        public bool IsNumberVerified { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsAccountVerified { get; set; }
        public string IdProofPath { get; set; }
        public string CancelledChequePath { get; set; }
        public string Image { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string Token { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile ProfileImage { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile IdProof { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile CancelledCheque { get; set; }
    }
}
