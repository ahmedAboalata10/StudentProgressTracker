namespace StudentProgress.API.DTOs.Analytics
{
    public class ProgressTrendDto
    {
        public string Period { get; set; }
        public double AvgCompletion { get; set; }
        public int AssessmentCount { get; set; }
    }

}
