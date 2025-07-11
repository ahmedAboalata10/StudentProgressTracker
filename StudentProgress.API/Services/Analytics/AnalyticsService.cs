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
using Microsoft.Extensions.Caching.Memory;

namespace StudentProgress.API.Services.Analytics
{

    public class AnalyticsService : IAnalyticsService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IMemoryCache _cache;
        public AnalyticsService(
            IStudentRepository studentRepo,
            IMemoryCache cache
            )
        {
            _studentRepo = studentRepo;
            _cache = cache;
        }

        public async Task<PagedResult<ClassSummaryDto>> GetClassSummaryAsync(PagingParameters paging)
        {
            const string cacheKey = "class_summary_data";

            if (!_cache.TryGetValue(cacheKey, out List<ClassSummaryDto> grouped))
            {
                var students = await _studentRepo.GetAllWithProgressAsync();

                grouped = students
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

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // expire after 10 mins
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _cache.Set(cacheKey, grouped, cacheOptions);
            }

            var totalCount = grouped.Count;
            var items = grouped.Skip(paging.Skip).Take(paging.PageSize).ToList();

            return new PagedResult<ClassSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
        }

        public async Task<PagedResult<ProgressTrendDto>> GetProgressTrendsAsync(PagingParameters paging)
        {
            const string cacheKey = "progress_trends_data";

            if (!_cache.TryGetValue(cacheKey, out List<ProgressTrendDto> grouped))
            {
                var students = await _studentRepo.GetAllWithProgressAsync();

                var progressRecords = students.SelectMany(s => s.ProgressRecords)
                    .Where(p => p.InsertAt >= DateTime.UtcNow.AddMonths(-6))
                    .ToList();

                grouped = progressRecords
                    .GroupBy(p => p.InsertAt.ToString("yyyy-MM"))
                    .Select(g => new ProgressTrendDto
                    {
                        Period = g.Key,
                        AvgCompletion = g.Average(p => p.CompletionPercent),
                        AssessmentCount = g.Count()
                    })
                    .ToList();

                _cache.Set(cacheKey, grouped, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            var totalCount = grouped.Count;
            var items = grouped.Skip(paging.Skip).Take(paging.PageSize).ToList();

            return new PagedResult<ProgressTrendDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
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
