using Health_Insurance.Data;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminEmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminEmployeeController(ApplicationDbContext context,
                                       UserManager<ApplicationUser> userManager,
                                       RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //  List all employees
        public async Task<IActionResult> Index()
        {
            var employees = await _context.EmpRegisters.ToListAsync();
            ViewBag.TotalEmployees = employees.Count;
            return View(employees);
        }

        //  Create Employee (GET)
        public IActionResult Create()
        {
            return View();
        }

        //  Create Employee (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpRegister model, string password)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingUser = await _userManager.FindByEmailAsync(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "This email is already registered.");
                    return View(model);
                }

                // Create user in Identity
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Username,
                    FullName = model.FirstName + " " + model.LastName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Ensure role exists
                    if (!await _roleManager.RoleExistsAsync("Employee"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Employee"));
                    }

                    // Assign role
                    await _userManager.AddToRoleAsync(user, "Employee");

                    // Add record in EmpRegister table
                    model.Password = password; // optional store
                    _context.EmpRegisters.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Employee registered successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        //  Details
        public async Task<IActionResult> Details(int id)
        {
            var emp = await _context.EmpRegisters.FindAsync(id);
            if (emp == null)
                return NotFound();
            return View(emp);
        }

        //  Edit Employee (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var emp = await _context.EmpRegisters.FindAsync(id);
            if (emp == null)
                return NotFound();
            return View(emp);
        }

        //  Edit Employee (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmpRegister model, string? newPassword)
        {
            if (id != model.EmpNo)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Update basic info
                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    //  If new password provided then update in Identity user
                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        var user = await _userManager.FindByEmailAsync(model.Username);
                        if (user != null)
                        {
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                            if (!result.Succeeded)
                            {
                                foreach (var error in result.Errors)
                                    ModelState.AddModelError("", error.Description);
                                return View(model);
                            }
                        }
                    }

                    TempData["Success"] = "Employee updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while updating employee.");
                }
            }

            return View(model);
        }


        //  Delete (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var emp = await _context.EmpRegisters.FindAsync(id);
            if (emp == null)
                return NotFound();
            return View(emp);
        }

        //  Delete (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emp = await _context.EmpRegisters.FindAsync(id);
            if (emp != null)
            {
                // Delete Identity user too
                var user = await _userManager.FindByEmailAsync(emp.Username);
                if (user != null)
                    await _userManager.DeleteAsync(user);

                _context.EmpRegisters.Remove(emp);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Employee deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
