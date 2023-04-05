using Jobify.Models;

namespace Jobify.ViewModels
{
    public class JobDetailsViewModel
    {
        public ApplyViewModel Opportunity { get; set; }
        public List<Job> RelevantsJob { get; set; }
    }
}
