
using Health_Insurance.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Health_Insurance.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmpRegister> EmpRegisters { get; set; } = null!;
        public DbSet<CompanyDetails> CompanyDetails { get; set; } = null!;
        public DbSet<Policy> Policies { get; set; } = null!;
        public DbSet<HospitalInfo> HospitalInfos { get; set; } = null!;
        public DbSet<PoliciesOnEmployees> PoliciesOnEmployees { get; set; } = null!;
        public DbSet<PolicyApprovalDetails> PolicyApprovalDetails { get; set; } = null!;
        public DbSet<PolicyRequestDetails> PolicyRequestDetails { get; set; } = null!;
    }

}
