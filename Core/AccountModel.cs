using System;

namespace Core
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string AlternateNumber { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public int? RoleId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string CompanyName { get; set; }
        public bool? IsVerified { get; set; }
    }
}
