using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Insurance.Models
{
    public class PolicyApprovalDetails
    {
        [Key]
        public int Id { get; set; }

        public int? PolicyId { get; set; }
        [ForeignKey(nameof(PolicyId))]
        public Policy? Policy { get; set; }

        public int? RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public PolicyRequestDetails? Request { get; set; }

        public DateTime? Date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; } // Approved / Rejected / Pending

        [MaxLength(500)]
        public string? Reason { get; set; }
    }
}
