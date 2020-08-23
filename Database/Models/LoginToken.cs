using System;

namespace Database.Models {
    public class LoginToken {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDt { get; set; }
    }
}
