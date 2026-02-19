
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Health_Insurance.Models
{
    public class PoliciesOnEmployees
    {
        [Key]
        public int Id { get; set; } // record id

        // EmpNo FK
        public int EmpNo { get; set; }
        [ForeignKey(nameof(EmpNo))]
        public EmpRegister? Employee { get; set; }

        // Policy FK
        public int? PolicyId { get; set; }
        [ForeignKey(nameof(PolicyId))]
        public Policy? Policy { get; set; }

        // Denormalized fields (as per your spec)
        [MaxLength(150)]
        public string? PolicyName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PolicyAmount { get; set; }

        public int? PolicyDuration { get; set; } // in months/years as you decide

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Emi { get; set; }

        public DateTime? PStartDate { get; set; }
        public DateTime? PEndDate { get; set; }

        // company info duplicated
        public int? CompanyId { get; set; }
        [MaxLength(150)]
        public string? CompanyName { get; set; }

        [MaxLength(150)]
        public string? Medical { get; set; } // could store hospital name or id as string

        public string? Status { get; set; } // Active/Expired/Cancelled etc.
        public string? Remarks { get; set; }
    }

}
