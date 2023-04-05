using Jobify.Data;
using System.ComponentModel.DataAnnotations;

namespace Jobify.Models
{
    public class ApplyForJob
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime ApplyDate { get; set; }
        public Job Job { get; set; }
        public int JobId { get; set; }
        public ApplicationUser Employee { get; set; }
        public string EmployeeId { get; set; }
    }
}
