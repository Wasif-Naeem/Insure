using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Insure.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeNo { get; set; }

        [Display(Name = "Designation")]
        public string? Designation { get; set; }

        [Display(Name = "Join Date")]
        public DateTime? JoinDate { get; set; }

        [Display(Name = "Employee Salary")]
        public float? Salary { get; set; }

        [Required]

        [Display(Name = "First Name")]
        [MaxLength(50)]

        public string FirstName { get; set; }

        [Required]

        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]

        [Display(Name = "User Name")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]

        [Display(Name = "Password")]
        [MaxLength(50)]
        public string Password { get; set; }

        [Display(Name = "Address")]
        [MaxLength(150)]
        public string? Address { get; set; }

        [Display(Name = "Phone Number")]
        public string? ContactNo { get; set; }

        [Display(Name = "State")]
        public string? State { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }

        [Display(Name = "City")]
        public string? City { get; set; }





    }
}
