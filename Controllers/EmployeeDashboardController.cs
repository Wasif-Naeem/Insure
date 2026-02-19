using Health_Insurance.Data;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Health_Insurance.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeDashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ===============================
        // DASHBOARD (Main Page)
        // ===============================
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var emp = await _context.EmpRegisters.FirstOrDefaultAsync(e => e.Username == userEmail);

            if (emp == null)
                return RedirectToAction("Login", "Account");

            // Stats
            int activePolicies = await _context.PolicyRequestDetails
                .CountAsync(r => r.EmpNo == emp.EmpNo && r.Status == "Approved");

            int pendingRequests = await _context.PolicyRequestDetails
                .CountAsync(r => r.EmpNo == emp.EmpNo && r.Status == "Pending");

            int rejectedPolicies = await _context.PolicyRequestDetails
                .CountAsync(r => r.EmpNo == emp.EmpNo && r.Status == "Rejected");

            // Recent 5 requests
            var recentRequests = await _context.PolicyRequestDetails
                .Include(r => r.Policy)
                .Where(r => r.EmpNo == emp.EmpNo)
                .OrderByDescending(r => r.RequestDate)
                .Take(5)
                .ToListAsync();

            ViewBag.ActivePolicies = activePolicies;
            ViewBag.PendingRequests = pendingRequests;
            ViewBag.RejectedPolicies = rejectedPolicies;

            return View(recentRequests);
        }

        // ===============================
        // EDIT PROFILE
        // ===============================
        public async Task<IActionResult> EditProfile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var emp = await _context.EmpRegisters.FirstOrDefaultAsync(e => e.Username == userEmail);
            if (emp == null)
                return NotFound();
            return View(emp);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EmpRegister model)
        {
            if (ModelState.IsValid)
            {
                var emp = await _context.EmpRegisters.FindAsync(model.EmpNo);
                if (emp == null) return NotFound();

                emp.FirstName = model.FirstName;
                emp.LastName = model.LastName;
                emp.Designation = model.Designation;
                emp.Address = model.Address;
                emp.ContactNo = model.ContactNo;
                emp.State = model.State;
                emp.Country = model.Country;
                emp.City = model.City;
                emp.Salary = model.Salary;

                _context.Update(emp);
                await _context.SaveChangesAsync();

                TempData["success"] = "Profile updated successfully!";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // ===============================
        // CHANGE PASSWORD
        // ===============================
        public IActionResult ChangePassword() => View();

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
            {
                ViewBag.Success = "Password changed successfully!";
            }
            else
            {
                ViewBag.Errors = result.Errors.Select(e => e.Description).ToList();
            }
            return View();
        }

        // ===============================
        // SEARCH POLICIES
        // ===============================
        public IActionResult SearchPolicies()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchPolicies(string keyword)
        {
            var policies = await _context.Policies
                .Include(p => p.Company)
                .Include(p => p.Medical)
                .Where(p =>
                    string.IsNullOrEmpty(keyword) ||
                    p.PolicyName.Contains(keyword) ||
                    p.PolicyDesc.Contains(keyword) ||
                    p.Company.CompanyName.Contains(keyword))
                .ToListAsync();

            return View("SearchResults", policies);
        }

        // ===============================
        // REQUEST A POLICY
        // ===============================
        [HttpPost]
        public async Task<IActionResult> RequestPolicy(int policyId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var emp = await _context.EmpRegisters.FirstOrDefaultAsync(e => e.Username == userEmail);
            if (emp == null) return NotFound();

            var policy = await _context.Policies
                .Include(p => p.Company)
                .FirstOrDefaultAsync(p => p.PolicyId == policyId);
            if (policy == null) return NotFound();

            // Prevent duplicate pending request
            var alreadyRequested = await _context.PolicyRequestDetails
                .AnyAsync(r => r.EmpNo == emp.EmpNo && r.PolicyId == policyId && r.Status == "Pending");

            if (alreadyRequested)
            {
                TempData["warning"] = "You already have a pending request for this policy!";
                return RedirectToAction("SearchPolicies");
            }

            var req = new PolicyRequestDetails
            {
                RequestDate = DateTime.Now,
                EmpNo = emp.EmpNo,
                PolicyId = policy.PolicyId,
                PolicyName = policy.PolicyName,
                PolicyAmount = policy.Amount,
                Emi = policy.Emi,
                CompanyId = policy.CompanyId,
                CompanyName = policy.Company?.CompanyName,
                Status = "Pending"
            };

            _context.PolicyRequestDetails.Add(req);
            await _context.SaveChangesAsync();

            TempData["success"] = "Policy request sent to Admin!";
            return RedirectToAction("MyRequests");
        }

        // ===============================
        // MY REQUESTS (All Policy Requests)
        // ===============================
        public async Task<IActionResult> MyRequests()
        {
            var email = User.Identity?.Name;
            var emp = await _context.EmpRegisters.FirstOrDefaultAsync(e => e.Username == email);
            if (emp == null) return NotFound();

            var requests = await _context.PolicyRequestDetails
                .Include(r => r.Policy)
                .Include(r => r.Policy.Company)
                .Where(r => r.EmpNo == emp.EmpNo)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();

            return View(requests);
        }
    }
}
