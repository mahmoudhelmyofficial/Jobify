using Jobify.Data;
using Jobify.Models;
using Jobify.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Controllers.User
{
    [Authorize]
    public class ApplyForJobsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public ApplyForJobsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Corporate")]
        public async Task<ActionResult> AllRequestes()
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (currentUser is null) return NotFound();

            var jobs = context.ApplyForJobs
                .Where(j => j.Job.JobPublisher == currentUser.UserName)
                .Include(j => j.Employee).Include(r => r.Job).ToList();

            return View(jobs);
        }

        [Authorize(Roles = "Corporate")]
        public async Task<ActionResult> Requestes(int id)
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (currentUser is null) return NotFound();

            var jobs = context.ApplyForJobs
                .Where(j => j.Job.JobPublisher == currentUser.UserName
                    && j.Job.Id == id)
                .Include(j => j.Employee).Include(r => r.Job).ToList();

            return View(jobs);
        }

        public async Task<ActionResult> Index()
        {
            var currentUser = userManager.FindByNameAsync(User.Identity.Name);

            var jobs = context.ApplyForJobs
                .Where(j => j.Employee.Id == currentUser.Result.Id)
                .Include(j => j.Job).ToList();

            return View(jobs);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> opportunityDetials(int id)
        {
            var job = await context.Job.Include(c => c.Category).FirstOrDefaultAsync(i => i.Id == id);

            //var job = await context.Job.Include().FirstOrDefaultAsync(i => i.Id == id);

            var publisher = await userManager.FindByNameAsync(job.JobPublisher);

            var applicants = context.ApplyForJobs.Where(j => j.JobId == job.Id).ToList().Count;

            var currentUser = await userManager.FindByNameAsync(User.Identity?.Name);

            JobDetailsViewModel jobVm = new JobDetailsViewModel();

            jobVm.Opportunity = new ApplyViewModel()
            {
                JobId = job.Id,
                Title = job.Title,
                PublishDate = job.PublishDate,
                ApplicantsCount = applicants,
                CareerLevel = job.CareerLevel,
                EducationLevel = job.EducationLevel,
                Experience = job.Experience,
                Salary = job.Salary,
                Category = job.Category.Name,
                JobPublisher = publisher,
                Positoins=job.OpenPositions
            };

            var applyed = context.ApplyForJobs.FirstOrDefault(j => j.Job.Id == job.Id
                    && j.EmployeeId == currentUser.Id);

            if (applyed != null) jobVm.Opportunity.Applyed = true;

            var savedJob = context.SavedJobs.FirstOrDefault(j => j.JobId == job.Id && j.UserName == currentUser.UserName);

            if (savedJob is not null) jobVm.Opportunity.SavedJob = true;

            //RELEVANT JOBS

            var res = context.Job.Where(j => j.Category.Name == job.Category.Name
                        && j.Id != job.Id)
                    .Include(j => j.Category).ToList();

            jobVm.RelevantsJob = res;


            return View(jobVm);
        }


        [HttpGet]
        //[Authorize(Roles = "Employee")]
        public async Task<IActionResult> Apply(int id)
        {
            var job = await context.Job.FindAsync(id);

            var currentUser = userManager.FindByNameAsync(User.Identity?.Name);

            var applyjob = new ApplyForJob { Job = job };

            if (currentUser.Result.EmployeeCV != null)
            {
                //applyjob.HasCV = true;
            }


            return View(applyjob);
        }


        [HttpPost]
        //[AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Apply(ApplyForJob jobApplyed)
        {
            var currentUser = userManager.FindByNameAsync(User.Identity?.Name);

            var job = new ApplyForJob
            {
                JobId = jobApplyed.Job.Id,
                Message = jobApplyed.Message,
                EmployeeId = currentUser.Result.Id,
                ApplyDate = DateTime.Now,
            };

            await context.ApplyForJobs.AddAsync(job);
            context.SaveChanges();

            return RedirectToAction(nameof(Index), "Jobs");
        }



        // POST: ApplyForJobs/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {

            var job = await context.Job.FindAsync(id);

            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            var apply = await context.ApplyForJobs
                .FirstOrDefaultAsync(a => a.JobId == job.Id && a.EmployeeId == currentUser.Id);


            if (apply != null)
            {
                context.ApplyForJobs.Remove(apply);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
