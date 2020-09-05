using System.ComponentModel.DataAnnotations;

namespace Database.Models {
    public class State {
        [Key]
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }
    }
}
