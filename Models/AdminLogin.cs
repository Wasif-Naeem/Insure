
using System.ComponentModel.DataAnnotations;
namespace Health_Insurance.Models
{
public class AdminLogin
{
    [Key]
    public int AdminId { get; set; }

    [Required, MaxLength(100)]
    public string Username { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Password { get; set; } = null!; // store hashed in real app
}

}
