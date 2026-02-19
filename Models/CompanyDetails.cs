using System.ComponentModel.DataAnnotations;

namespace Health_Insurance.Models
{
    public class CompanyDetails
    {
        [Key]
        public int CompanyId { get; set; }

        [Required, MaxLength(150)]
        public string CompanyName { get; set; } = null!;

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }

        [Url(ErrorMessage = "Company URL must be a valid URL")]
        public string? CompanyUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Policy>? Policies { get; set; }
        public ICollection<EmpRegister>? Employees { get; set; }
    }

}
