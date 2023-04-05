using Jobify.Data;
using Jobify.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Controllers.Corporate
{
    public class CorporateController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly IWebHostEnvironment hosting;

        public CorporateController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
             IWebHostEnvironment hosting)
        {
            this.context = context;
            this.userManager = userManager;
            this.userStore = userStore;
            this.hosting = hosting;
        }

        public async Task<IActionResult> Profile(string publisher)
        {
            var employer = await userManager.FindByNameAsync(publisher);

            var jobs = await context.Job.Where(j => j.JobPublisher == publisher).Include(c => c.Category).ToListAsync();

            var profile = new CorporateProfileVM
            {
                UserName = publisher,
                Location = employer.Location,
                Industry = employer.Industry,
                CompanySize = employer.CompanySize,
                Specialities = employer.Specialities,
                Vacancies = jobs,
                ProfilePicture =employer.ProfilePicture
            };

            return View(profile);
        }

        public async Task<IActionResult> Update(string user)
        {
            var porfile = await userManager.FindByNameAsync(user);
            return View(porfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ApplicationUser user)
        {
            var prof = await userManager.FindByNameAsync(User.Identity.Name);

            prof.Industry = user.Industry;
            prof.Location = user.Location;
            prof.CompanySize = user.CompanySize;
            prof.Specialities = user.Specialities;

            using var dataStream = new MemoryStream();

            if (prof.ProfilePicture != dataStream.ToArray())
            {
                var file = Request.Form.Files;
                var poster = file.FirstOrDefault();
                if (file.Any())
                    await poster.CopyToAsync(dataStream);
                prof.ProfilePicture = dataStream.ToArray();
            }

            await userStore.SetUserNameAsync(prof, user.UserName, CancellationToken.None);
            await userManager.UpdateAsync(prof);

            return RedirectToAction(nameof(Index), "Jobs");

        }


        public ActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(CorporateRegisterVM corporateVM)
        {
            var corporate = new ApplicationUser
            {
                UserName = corporateVM.UserName,
                PhoneNumber = corporateVM.PhoneNumber,
                Email = corporateVM.Email
            };

            await userStore.SetUserNameAsync(corporate, corporate.UserName, CancellationToken.None);

            var result = await userManager.CreateAsync(corporate, corporateVM.Password);

            await userManager.AddToRoleAsync(corporate, "Corporate");

            return RedirectToAction("Index", "Home");

        }
    }
}