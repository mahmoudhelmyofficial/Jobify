using System.ComponentModel.DataAnnotations;

namespace Jobify.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Experience { get; set; }
        public string CareerLevel { get; set; }
        public string EducationLevel { get; set; }
        public float Salary { get; set; }
        public int OpenPositions { get; set; }
        public JobCategory Category { get; set; }
        public int CategoryId { get; set; }
        public string JobPublisher { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PublishDate { get; set; }

    }
}
