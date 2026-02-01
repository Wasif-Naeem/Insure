using System.ComponentModel.DataAnnotations;

namespace Insure.Models
{
    public class CompanyDetails
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Address")]
        public string? Address { get; set; }

        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [Display(Name = "Company Url")]
        public string? CompanyUrl { get;set;  }
    }
}
