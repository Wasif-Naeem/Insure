using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Health_Insurance.Models
{
   

    public class PolicyRequestDetails
    {
        [Key]
        public int RequestId { get; set; }

        public DateTime? RequestDate { get; set; }

        // Emp FK
        public int EmpNo { get; set; }
        [ForeignKey(nameof(EmpNo))]
        public EmpRegister? Employee { get; set; }

        // Policy FK
        public int? PolicyId { get; set; }
        [ForeignKey(nameof(PolicyId))]
        public Policy? Policy { get; set; }

        [MaxLength(150)]
        public string? PolicyName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PolicyAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Emi { get; set; }

        public int? CompanyId { get; set; }
        [MaxLength(150)]
        public string? CompanyName { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; } // Pending/Approved/Rejected

        [MaxLength(500)]
        public string? AdminRemarks { get; set; }
    }

}
