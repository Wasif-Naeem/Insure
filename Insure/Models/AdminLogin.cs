using System.ComponentModel.DataAnnotations;

namespace Insure.Models
{
    public class AdminLogin
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
