using Health_Insurance.Data;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PoliciesOnEmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PoliciesOnEmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  LIST ALL
        public async Task<IActionResult> Index()
        {
            var list = await _context.PoliciesOnEmployees
                .Include(p => p.Employee)
                .Include(p => p.Policy)
                .ToListAsync();

            return View(list);
        }

        //  CREATE GET
        public IActionResult Create()
        {
            ViewBag.Employees = new SelectList(_context.EmpRegisters, "EmpNo", "FirstName");
            ViewBag.Policies = new SelectList(_context.Policies, "PolicyId", "PolicyName");
            ViewBag.Companies = new SelectList(_context.CompanyDetails, "CompanyId", "CompanyName");
            return View();
        }

        //  CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PoliciesOnEmployees model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _context.EmpRegisters.FindAsync(model.EmpNo);
                var policy = await _context.Policies
                    .Include(p => p.Company)
                    .Include(p => p.Medical)
                    .FirstOrDefaultAsync(p => p.PolicyId == model.PolicyId);

                if (policy != null)
                {
                    model.PolicyName = policy.PolicyName;
                    model.PolicyAmount = policy.Amount;
                    model.Emi = policy.Emi;
                    model.CompanyId = policy.CompanyId;
                    model.CompanyName = policy.Company?.CompanyName;
                    model.Medical = policy.Medical?.HospitalName;
                    model.Status = "Active";
                }

                _context.PoliciesOnEmployees.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Policy assigned to employee successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Employees = new SelectList(_context.EmpRegisters, "EmpNo", "FirstName", model.EmpNo);
            ViewBag.Policies = new SelectList(_context.Policies, "PolicyId", "PolicyName", model.PolicyId);
            ViewBag.Companies = new SelectList(_context.CompanyDetails, "CompanyId", "CompanyName", model.CompanyId);
            return View(model);
        }

        //  EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _context.PoliciesOnEmployees.FindAsync(id);
            if (data == null) return NotFound();

            ViewBag.Employees = new SelectList(_context.EmpRegisters, "EmpNo", "FirstName", data.EmpNo);
            ViewBag.Policies = new SelectList(_context.Policies, "PolicyId", "PolicyName", data.PolicyId);
            return View(data);
        }

        //  EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PoliciesOnEmployees model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Record updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //  DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var data = await _context.PoliciesOnEmployees
                .Include(p => p.Employee)
                .Include(p => p.Policy)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (data == null) return NotFound();
            return View(data);
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.PoliciesOnEmployees
                .Include(p => p.Employee)
                .Include(p => p.Policy)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (data == null) return NotFound();
            return View(data);
        }

        //  DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var data = await _context.PoliciesOnEmployees.FindAsync(id);
            if (data != null)
            {
                _context.PoliciesOnEmployees.Remove(data);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Record deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
