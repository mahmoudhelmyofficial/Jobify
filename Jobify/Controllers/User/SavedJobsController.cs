using Jobify.Data;
using Jobify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Jobify.Controllers.User
{
    [Authorize]
    public class SavedJobsController : Controller
    {
        private readonly ApplicationDbContext context;

        public SavedJobsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var jobs = new List<Job>();

            if (User.Identity is not null)
            {
                var saved =await context.SavedJobs.Where(s => s.UserName == User.Identity.Name).ToListAsync();

                foreach (var item in saved)
                {
                    var job = await context.Job.Include(c => c.Category).FirstOrDefaultAsync(j => j.Id == item.JobId);
                    if (job is not null) jobs.Add(job);
                }

                return View(jobs);
            }

            else return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Save(int? id)
        {
            if (id == null || context.Job == null) return NotFound();

            var job = await context.Job.Include(j => j.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (job == null) return NotFound();

            var saved = context.SavedJobs.FirstOrDefault(j => j.JobId == id);

            if (saved is not null && saved.UserName == User.Identity.Name)
            {
                context.SavedJobs.Remove(saved);
                context.SaveChanges();
            }
            else
            {
                SavedJobs savedJob = new SavedJobs
                {
                    JobId = job.Id,
                    UserName = User.Identity.Name
                };

                context.SavedJobs.Add(savedJob);
                context.SaveChanges();
            }
            
            return RedirectToAction(nameof(Index));
        }

    }
}
