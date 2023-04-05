using Microsoft.AspNetCore.Identity;

namespace Jobify.ViewModels
{
    public class RolesViewModel
    {
        public IdentityRole Role { get; set; }
        public ICollection<IdentityRole> Roles { get; set; }
    }
}
