using Jobify.Models;

namespace Jobify.ViewModels
{
    public class CorporateProfileVM
    {
        public string? UserName { get; set; }
        public int PhoneNumber { get; set; }
        public string? WebSite { get; set; }
        public string? Location { get; set; }
        public int? Founded { get; set; }
        public string? Industry { get; set; }
        public int? CompanySize { get; set; }
        public string? Specialities { get; set; }
        public byte[] ProfilePicture { get; set; }
        public IEnumerable<Job>? Vacancies { get; set; }

    }
}
