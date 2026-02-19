using Health_Insurance.Data;
using Health_Insurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApprovalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ApprovalsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // ✅ List of Pending Requests for Admin
        public async Task<IActionResult> PendingRequests()
        {
            var requests = await _context.PolicyRequestDetails
                .Include(r => r.Policy)
                .Include(r => r.Policy.Company)
                .Include(r => r.Employee)
                .Where(r => r.Status == "Pending")
                .ToListAsync();

            return View(requests);
        }

        // ✅ Approve (GET)
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _context.PolicyRequestDetails
                .Include(r => r.Policy)
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // ✅ Approve (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string adminRemarks)
        {
            var request = await _context.PolicyRequestDetails.FindAsync(id);
            if (request == null) return NotFound();

            request.Status = "Approved";
            request.AdminRemarks = adminRemarks;
            await _context.SaveChangesAsync();

            TempData["success"] = "Policy request approved successfully!";
            return RedirectToAction(nameof(PendingRequests));
        }

        // ✅ Reject (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string adminRemarks)
        {
            var request = await _context.PolicyRequestDetails.FindAsync(id);
            if (request == null) return NotFound();

            request.Status = "Rejected";
            request.AdminRemarks = adminRemarks;
            await _context.SaveChangesAsync();

            TempData["error"] = "Policy request rejected!";
            return RedirectToAction(nameof(PendingRequests));
        }

        // List all approvals
        public async Task<IActionResult> Index()
        {
            var approvals = await _context.PolicyApprovalDetails
                .Include(a => a.Policy)
                .Include(a => a.Request)
                .ToListAsync();

            return View(approvals);
        }

        // Create - GET
        public IActionResult Create()
        {
            ViewBag.PolicyList = new SelectList(_context.Policies, "PolicyId", "PolicyName");
            ViewBag.RequestList = new SelectList(_context.PolicyRequestDetails, "RequestId", "PolicyName");
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PolicyApprovalDetails approval)
        {
            if (ModelState.IsValid)
            {
                approval.Date = DateTime.Now;
                _context.PolicyApprovalDetails.Add(approval);
                await _context.SaveChangesAsync();
                TempData["success"] = "Approval record added successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PolicyList = new SelectList(_context.Policies, "PolicyId", "PolicyName", approval.PolicyId);
            ViewBag.RequestList = new SelectList(_context.PolicyRequestDetails, "RequestId", "PolicyName", approval.RequestId);
            return View(approval);
        }

        // Edit - GET
        public async Task<IActionResult> Edit(int id)
        {
            var approval = await _context.PolicyApprovalDetails.FindAsync(id);
            if (approval == null) return NotFound();

            ViewBag.PolicyList = new SelectList(_context.Policies, "PolicyId", "PolicyName", approval.PolicyId);
            ViewBag.RequestList = new SelectList(_context.PolicyRequestDetails, "RequestId", "PolicyName", approval.RequestId);
            return View(approval);
        }

        // Edit - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PolicyApprovalDetails approval)
        {
            if (id != approval.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.PolicyApprovalDetails.Update(approval);
                await _context.SaveChangesAsync();
                TempData["success"] = "Approval record updated!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PolicyList = new SelectList(_context.Policies, "PolicyId", "PolicyName", approval.PolicyId);
            ViewBag.RequestList = new SelectList(_context.PolicyRequestDetails, "RequestId", "PolicyName", approval.RequestId);
            return View(approval);
        }

        // Delete - GET
        public async Task<IActionResult> Delete(int id)
        {
            var approval = await _context.PolicyApprovalDetails
                .Include(a => a.Policy)
                .Include(a => a.Request)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (approval == null) return NotFound();
            return View(approval);
        }

        // Delete - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var approval = await _context.PolicyApprovalDetails.FindAsync(id);
            if (approval == null) return NotFound();

            _context.PolicyApprovalDetails.Remove(approval);
            await _context.SaveChangesAsync();
            TempData["success"] = "Record deleted!";
            return RedirectToAction(nameof(Index));
        }
    }
}
