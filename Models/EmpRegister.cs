using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Health_Insurance.Models
{
 
    public class EmpRegister
    {
        [Key]
        public int EmpNo { get; set; } 

        [MaxLength(100)]
        public string? Designation { get; set; }

        public DateTime? JoinDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Salary { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(150)]
        public string? Username { get; set; }

        [MaxLength(200)]
        public string? Password { get; set; } 

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(30)]
        public string? ContactNo { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? PolicyStatus { get; set; } 

        
        public int? PolicyId { get; set; }
        [ForeignKey(nameof(PolicyId))]
        public Policy? Policy { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public CompanyDetails? Company { get; set; }

        public ICollection<PoliciesOnEmployees>? PoliciesOnEmployees { get; set; }
        public ICollection<PolicyRequestDetails>? PolicyRequests { get; set; }
    }
}
