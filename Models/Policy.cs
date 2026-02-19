using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Health_Insurance.Models
{
      public class Policy
    {
        [Key]
        public int PolicyId { get; set; }

        [Required, MaxLength(150)]
        public string PolicyName { get; set; } = null!;

        [MaxLength(1000)]
        public string? PolicyDesc { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Emi { get; set; }

        // FK to CompanyDetails
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public CompanyDetails? Company { get; set; }

        // MedicalId references HospitalInfo.HospitalId (medical provider)
        public int? MedicalId { get; set; }
        [ForeignKey(nameof(MedicalId))]
        public HospitalInfo? Medical { get; set; }

        // Optional request date (you said add requestDate to policies list)
        public DateTime? RequestDate { get; set; }

        // Navigation
        public ICollection<PoliciesOnEmployees>? PoliciesOnEmployees { get; set; }
        public ICollection<PolicyRequestDetails>? PolicyRequests { get; set; }
    }

}
