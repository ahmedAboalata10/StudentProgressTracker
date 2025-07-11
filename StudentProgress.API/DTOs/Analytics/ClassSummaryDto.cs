namespace StudentProgress.API.DTOs.Analytics
{
    public class ClassSummaryDto
    {
        public string Grade { get; set; }
        public int StudentCount { get; set; }
        public double AvgCompletion { get; set; }
        public double AvgPerformanceScore { get; set; }
        public TimeSpan AvgTimeSpent { get; set; }
    }


}
