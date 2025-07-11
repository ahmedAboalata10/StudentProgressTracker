using CsvHelper;
using Microsoft.EntityFrameworkCore;
using StudentProgress.API.Data;
using StudentProgress.API.DTOs;
using StudentProgress.API.DTOs.Analytics;
using StudentProgress.API.IRepositories;
using StudentProgress.API.IServices.Analytics;
using StudentProgress.API.Models.Students;
using System.Globalization;
using System.IO;
using System.Text;
namespace StudentProgress.API.Services.Analytics
{

    public class AnalyticsService : IAnalyticsService
    {
        private readonly IStudentRepository _studentRepo;

        public AnalyticsService(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        public async Task<PagedResult<ClassSummaryDto>> GetClassSummaryAsync(PagingParameters paging)
        {
            var students = await _studentRepo.GetAllWithProgressAsync();

            var grouped = students
                .GroupBy(s => s.Grade)
                .Select(g => new ClassSummaryDto
                {
                    Grade = g.Key,
                    StudentCount = g.Count(),
                    AvgCompletion = g.SelectMany(s => s.ProgressRecords).Average(p => p.CompletionPercent),
                    AvgPerformanceScore = g.SelectMany(s => s.ProgressRecords).Average(p => p.PerformanceScore),
                    AvgTimeSpent = TimeSpan.FromMinutes(
                        g.SelectMany(s => s.ProgressRecords).Average(p => p.TimeSpent.TotalMinutes))
                })
                .ToList();

            var totalCount = grouped.Count;

            var items = grouped
                .Skip(paging.Skip)
                .Take(paging.PageSize)
                .ToList();

            return new PagedResult<ClassSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
        }



        public async Task<List<ProgressTrendDto>> GetProgressTrendsAsync()
        {
            var students = await _studentRepo.GetAllWithProgressAsync();
            var progressRecords = students.SelectMany(s => s.ProgressRecords)
                .Where(p => p.InsertAt >= DateTime.UtcNow.AddMonths(-6))
                .ToList();

            return progressRecords
                .GroupBy(p => p.InsertAt.ToString("yyyy-MM"))
                .Select(g => new ProgressTrendDto
                {
                    Period = g.Key,
                    AvgCompletion = g.Average(p => p.CompletionPercent),
                    AssessmentCount = g.Count()
                }).ToList();
        }

        public async Task<byte[]> ExportStudentsAsCsvAsync()
        {
            var students = await _studentRepo.GetAllWithProgressAsync();

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteHeader<Student>();
            csv.NextRecord();

            foreach (var student in students)
            {
                csv.WriteRecord(student);
                csv.NextRecord();
            }

            writer.Flush();
            return memoryStream.ToArray();
        }
    }


}
