using System.ComponentModel.DataAnnotations;

namespace Database.Models {
    public class City {
        [Key]
        public int Id { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }
    }
}
