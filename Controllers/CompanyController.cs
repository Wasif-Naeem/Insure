using Health_Insurance.Data;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List
        public async Task<IActionResult> Index()
        {
            var companies = await _context.CompanyDetails.ToListAsync();
            return View(companies);
        }

        // Create - GET
        public IActionResult Create()
        {
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyDetails company)
        {
            if (ModelState.IsValid)
            {
                _context.CompanyDetails.Add(company);
                await _context.SaveChangesAsync();
                TempData["success"] = "Company added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // Edit - GET
        public async Task<IActionResult> Edit(int id)
        {
            var company = await _context.CompanyDetails.FindAsync(id);
            if (company == null) return NotFound();
            return View(company);
        }

        // Edit - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyDetails company)
        {
            if (id != company.CompanyId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.CompanyDetails.Update(company);
                await _context.SaveChangesAsync();
                TempData["success"] = "Company updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // Delete - GET
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _context.CompanyDetails.FindAsync(id);
            if (company == null) return NotFound();
            return View(company);
        }

        // Delete - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.CompanyDetails.FindAsync(id);
            if (company == null) return NotFound();

            _context.CompanyDetails.Remove(company);
            await _context.SaveChangesAsync();
            TempData["success"] = "Company deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
