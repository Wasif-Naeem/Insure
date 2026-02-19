using Microsoft.AspNetCore.Identity;

namespace Health_Insurance.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? EmpNo { get; set; } 
        public string? FullName { get; set; }
    }

}
