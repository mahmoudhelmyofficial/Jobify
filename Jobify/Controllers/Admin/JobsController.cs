using Jobify.Data;
using Jobify.Models;
using Jobify.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Jobify.Controllers.Admin
{
    public class JobsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public JobsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public IActionResult Index()
            => View(_context.Job.Include(j => j.Category).ToList());
        

        public async Task<IActionResult> Search(string term)
        {
            var res = _context.Job.Where(j => j.Title.Contains(term)
                || j.Description.Contains(term)).Include(j => j.Category).ToList();

            return View(nameof(Index), res);
        }
        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        [Authorize(Roles = "Corporate")]
        public IActionResult Create()
        {
            var categories = _context.JobCategory.ToList();

            var vm = new JobViewModel { Category = categories };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Corporate")]
        public async Task<IActionResult> Create(JobViewModel jobVM)
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            var job = new Job
            {
                Title = jobVM.Title,
                Description = jobVM.Description,
                CategoryId = jobVM.CategoryId,
                JobPublisher = currentUser.UserName,
                PublishDate = DateTime.Now,
                OpenPositions = jobVM.Positions,
                CareerLevel = jobVM.CareerLevel,
                EducationLevel = jobVM.EducationLevel,
                Experience = jobVM.Experience,
                Salary = jobVM.Salary
            };

            _context.Add(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.JobCategory, "Id", "Id", job.CategoryId);
            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,JobImage,Experience,CareerLevel,EducationLevel,Salary,OpenPositions,CategoryId,PublishDate")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.JobCategory, "Id", "Id", job.CategoryId);
            return View(job);
        }

        //// GET: Jobs/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Job == null)
        //    {
        //        return NotFound();
        //    }

        //    var job = await _context.Job
        //        .Include(j => j.Category)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(job);
        //}

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Job == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Job'  is null.");
            }
            var job = await _context.Job.FindAsync(id);
            if (job != null)
            {
                _context.Job.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return (_context.Job?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
