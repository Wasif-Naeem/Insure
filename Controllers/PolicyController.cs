using Health_Insurance.Data;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PolicyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PolicyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var policies = await _context.Policies
                .Include(p => p.Company)
                .Include(p => p.Medical)
                .ToListAsync();
            return View(policies);
        }

        // CREATE - GET
        public IActionResult Create()
        {
            ViewBag.CompanyList = new SelectList(_context.CompanyDetails, "CompanyId", "CompanyName");
            ViewBag.MedicalList = new SelectList(_context.HospitalInfos, "HospitalId", "HospitalName");
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Policy policy)
        {
            if (ModelState.IsValid)
            {
                policy.RequestDate = DateTime.Now; 
                _context.Policies.Add(policy);
                await _context.SaveChangesAsync();
                TempData["success"] = "Policy added successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CompanyList = new SelectList(_context.CompanyDetails, "CompanyId", "CompanyName", policy.CompanyId);
            ViewBag.MedicalList = new SelectList(_context.HospitalInfos, "HospitalId", "HospitalName", policy.MedicalId);
            return View(policy);
        }

        // EDIT - GET
        public async Task<IActionResult> Edit(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null) return NotFound();

            ViewBag.CompanyList = new SelectList(_context.CompanyDetails, "CompanyId", "CompanyName", policy.CompanyId);
            ViewBag.MedicalList = new SelectList(_context.HospitalInfos, "HospitalId", "HospitalName", policy.MedicalId);
            return View(policy);
        }

        // EDIT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Policy policy)
        {
            if (id != policy.PolicyId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Policies.Update(policy);
                await _context.SaveChangesAsync();
                TempData["success"] = "Policy updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CompanyList = new SelectList(_context.CompanyDetails, "CompanyId", "CompanyName", policy.CompanyId);
            ViewBag.MedicalList = new SelectList(_context.HospitalInfos, "HospitalId", "HospitalName", policy.MedicalId);
            return View(policy);
        }

        // DELETE - GET
        public async Task<IActionResult> Delete(int id)
        {
            var policy = await _context.Policies
                .Include(p => p.Company)
                .Include(p => p.Medical)
                .FirstOrDefaultAsync(p => p.PolicyId == id);

            if (policy == null) return NotFound();

            return View(policy);
        }

        // DELETE - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null) return NotFound();

            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync();
            TempData["success"] = "Policy deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
