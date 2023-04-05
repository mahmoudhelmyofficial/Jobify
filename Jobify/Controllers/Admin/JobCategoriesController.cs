using Jobify.Data;
using Jobify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class JobCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobCategories
        public async Task<IActionResult> Index()
        {
            return _context.JobCategory != null ?
                        View(await _context.JobCategory.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.JobCategory'  is null.");
        }

        // GET: JobCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JobCategory == null)
            {
                return NotFound();
            }

            var jobCategory = await _context.JobCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobCategory == null)
            {
                return NotFound();
            }

            return View(jobCategory);
        }

        // GET: JobCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobCategory jobCategory)
        {
            _context.Add(jobCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: JobCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JobCategory == null)
            {
                return NotFound();
            }

            var jobCategory = await _context.JobCategory.FindAsync(id);
            if (jobCategory == null)
            {
                return NotFound();
            }
            return View(jobCategory);
        }

        // POST: JobCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] JobCategory jobCategory)
        {
            if (id != jobCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobCategoryExists(jobCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobCategory);
        }

        // GET: JobCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JobCategory == null)
            {
                return NotFound();
            }

            var jobCategory = await _context.JobCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobCategory == null)
            {
                return NotFound();
            }

            return View(jobCategory);
        }

        // POST: JobCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.JobCategory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JobCategory'  is null.");
            }
            var jobCategory = await _context.JobCategory.FindAsync(id);
            if (jobCategory != null)
            {
                _context.JobCategory.Remove(jobCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobCategoryExists(int id)
        {
            return (_context.JobCategory?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
