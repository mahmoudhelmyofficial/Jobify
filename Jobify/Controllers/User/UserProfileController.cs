using Jobify.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.Controllers.User
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hosting;
        public UserProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment hosting)
        {
            this.userManager = userManager;
            this.hosting = hosting;
        }


        public async Task<IActionResult> Index()
            => View(await userManager.FindByNameAsync(User.Identity.Name));

        public async Task<IActionResult> Edit()
            => View(await userManager.FindByNameAsync(User.Identity.Name));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var prof = await userManager.FindByNameAsync(User.Identity.Name);

            prof.FirstName = user.FirstName;
            prof.LastName = user.LastName;
            prof.MiddleName = user.MiddleName;
            prof.Email = user.Email;
            prof.Nationality = user.Nationality;
            prof.MilitaryStatus = user.MilitaryStatus;
            prof.PhoneNumber = user.PhoneNumber;
            prof.Gender = user.Gender;

            await userManager.UpdateAsync(prof);

            using var dataStream = new MemoryStream();

            if (prof.ProfilePicture != dataStream.ToArray())
            {
                var file = Request.Form.Files;
                var poster = file.FirstOrDefault();
                if (file.Any())
                    await poster.CopyToAsync(dataStream);
                prof.ProfilePicture = dataStream.ToArray();

                await userManager.UpdateAsync(prof);
            }

            var cVfile = Request.Form.Files.LastOrDefault();
            var filename = string.Empty;

            if (cVfile != null && ".pdf".Contains(Path.GetExtension(cVfile.FileName).ToLower().Trim()))
            {
                string uplod = Path.Combine(hosting.WebRootPath, "CVs");
                filename = cVfile.FileName;
                string fullPath = Path.Combine(uplod, filename);
                cVfile.CopyTo(new FileStream(fullPath, FileMode.Create));

                prof.EmployeeCV = filename;
                await userManager.UpdateAsync(prof);
            }

            return RedirectToAction(nameof(Index), "Jobs");
        }


    }
}
