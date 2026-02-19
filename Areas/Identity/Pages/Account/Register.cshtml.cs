// Inside Register.cshtml.cs
using Health_Insurance.Data;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _db;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public string? Designation { get; set; }

        public string? Address { get; set; }

        [Display(Name = "Contact No")]
        public string? ContactNo { get; set; }

        public string? State { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }

        [Display(Name = "Salary")]
        [DataType(DataType.Currency)]
        public decimal? Salary { get; set; }

        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime? JoinDate { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = $"{Input.FirstName} {Input.LastName}"
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee");

                var emp = new EmpRegister
                {
                    Username = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Designation = Input.Designation,
                    Address = Input.Address,
                    ContactNo = Input.ContactNo,
                    State = Input.State,
                    Country = Input.Country,
                    City = Input.City,
                    Salary = Input.Salary,
                    JoinDate = Input.JoinDate
                };

                _db.EmpRegisters.Add(emp);
                await _db.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");   

            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return Page();
    }
}
