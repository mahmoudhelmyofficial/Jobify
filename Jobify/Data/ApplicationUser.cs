using Microsoft.AspNetCore.Identity;

namespace Jobify.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? MilitaryStatus { get; set; }
        public string? Nationality { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string? EmployeeCV { get; set; }

        //Corporate

        public string? WebSite { get; set; }
        public string? Location { get; set; }
        public int? Founded { get; set; }
        public string? Industry { get; set; }
        public int? CompanySize { get; set; }
        public string? Specialities { get; set; }
    }
}
