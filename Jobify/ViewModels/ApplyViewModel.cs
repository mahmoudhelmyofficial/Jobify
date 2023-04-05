using Jobify.Data;

namespace Jobify.ViewModels
{
    public class ApplyViewModel
    {
        public bool Applyed { get; set; }
        public bool SavedJob { get; set; }
        public int ApplicantsCount { get; set; }
        public int Positoins { get; set; }
        public ApplicationUser JobPublisher { get; set; }
        public int JobId { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string Category { get; set; }
        public string Experience { get; set; }
        public string CareerLevel { get; set; }
        public string EducationLevel { get; set; }
        public float Salary { get; set; }
    }
}
