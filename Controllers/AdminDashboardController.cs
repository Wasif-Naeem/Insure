using Health_Insurance.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Dashboard counts
            ViewBag.CompanyCount = await _context.CompanyDetails.CountAsync();
            ViewBag.PolicyCount = await _context.Policies.CountAsync();
            ViewBag.EmployeeCount = await _context.EmpRegisters.CountAsync();
            ViewBag.PendingRequests = await _context.PolicyRequestDetails
                                                   .CountAsync(r => r.Status == "Pending");

            var pendingRequests = await _context.PolicyRequestDetails
            .Include(r => r.Policy)
            .Include(r => r.Employee)
            .Where(r => r.Status == "Pending")
            .OrderByDescending(r => r.RequestDate)
            .Take(4)
            .ToListAsync();


            return View(pendingRequests);
        }

      
    }
}
