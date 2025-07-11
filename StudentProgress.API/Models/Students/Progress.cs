using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProgress.API.Models.Students
{
    public class Progress:BaseEntity
    {
        public string Subject { get; set; }
        public double CompletionPercent { get; set; }
        public double PerformanceScore { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public DateTime LastActivity { get; set; }
        [ForeignKey("Student")]
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
    }
}
