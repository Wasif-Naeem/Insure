using Health_Insurance.Data;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeePolicyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeePolicyController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Step 1: Search + List all policies
        public async Task<IActionResult> Index(string search)
        {
            var policies = _context.Policies.Include(p => p.Company).Include(p => p.Medical).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                policies = policies.Where(p => p.PolicyName.Contains(search) || p.Company.CompanyName.Contains(search));
            }

            return View(await policies.ToListAsync());
        }

        // Step 2: View policy details
        public async Task<IActionResult> Details(int id)
        {
            var policy = await _context.Policies
                .Include(p => p.Company)
                .Include(p => p.Medical)
                .FirstOrDefaultAsync(p => p.PolicyId == id);

            if (policy == null) return NotFound();
            return View(policy);
        }

        // Step 3: Send request
        [HttpPost]
        public async Task<IActionResult> RequestPolicy(int policyId)
        {
            var user = await _userManager.GetUserAsync(User);
            var emp = await _context.EmpRegisters.FirstOrDefaultAsync(e => e.Username == user.Email);

            if (emp == null)
            {
                TempData["error"] = "Employee record not found!";
                return RedirectToAction("Index");
            }

            var existing = await _context.PolicyRequestDetails
                .FirstOrDefaultAsync(r => r.PolicyId == policyId && r.EmpNo == emp.EmpNo && r.Status == "Pending");

            if (existing != null)
            {
                TempData["warning"] = "You already requested this policy and it’s pending!";
                return RedirectToAction("Index");
            }

            var request = new PolicyRequestDetails
            {
                PolicyId = policyId,
                EmpNo = emp.EmpNo,
                RequestDate = DateTime.Now,
                Status = "Pending"
            };

            _context.PolicyRequestDetails.Add(request);
            await _context.SaveChangesAsync();

            TempData["success"] = "Your request has been sent to Admin!";
            return RedirectToAction("MyRequests");
        }

        // Step 4: View my requests
        public async Task<IActionResult> MyRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            var emp = await _context.EmpRegisters.FirstOrDefaultAsync(e => e.Username == user.Email);

            var requests = await _context.PolicyRequestDetails
                .Include(r => r.Policy)
                .Include(r => r.Policy.Company)
                .Where(r => r.EmpNo == emp.EmpNo)
                .ToListAsync();

            return View(requests);
        }
    }
}
