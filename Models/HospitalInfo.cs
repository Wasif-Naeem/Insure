using System.ComponentModel.DataAnnotations;

namespace Health_Insurance.Models
{
    public class HospitalInfo
    {
        [Key]
        public int HospitalId { get; set; }

        [Required, MaxLength(150)]
        public string HospitalName { get; set; } = null!;

        [MaxLength(30)]
        public string? PhoneNo { get; set; }

        [MaxLength(300)]
        public string? Location { get; set; }

        // As per your requirement: accept URL/file references — validated as URL
        [Url(ErrorMessage = "Hospital URL must be a valid URL")]
        public string? Url { get; set; }
    }

}
