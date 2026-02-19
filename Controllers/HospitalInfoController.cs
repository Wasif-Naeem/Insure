using Health_Insurance.Data;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HospitalInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all hospitals
        public async Task<IActionResult> Index()
        {
            var hospitals = await _context.HospitalInfos.ToListAsync();
            return View(hospitals);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HospitalInfo model)
        {
            if (ModelState.IsValid)
            {
                _context.HospitalInfos.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Hospital added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var hospital = await _context.HospitalInfos.FindAsync(id);
            if (hospital == null) return NotFound();
            return View(hospital);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HospitalInfo model)
        {
            if (id != model.HospitalId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Hospital updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            var hospital = await _context.HospitalInfos.FindAsync(id);
            if (hospital == null) return NotFound();

            return View(hospital);
        }

        // POST: Delete Confirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospital = await _context.HospitalInfos.FindAsync(id);
            if (hospital != null)
            {
                _context.HospitalInfos.Remove(hospital);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Hospital deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        // View Details
        public async Task<IActionResult> Details(int id)
        {
            var hospital = await _context.HospitalInfos.FindAsync(id);
            if (hospital == null) return NotFound();
            return View(hospital);
        }
    }
}
