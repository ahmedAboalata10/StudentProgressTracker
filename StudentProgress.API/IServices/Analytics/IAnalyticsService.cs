using StudentProgress.API.DTOs;
using StudentProgress.API.DTOs.Analytics;

namespace StudentProgress.API.IServices.Analytics
{
    public interface IAnalyticsService
    {
        Task<PagedResult<ClassSummaryDto>> GetClassSummaryAsync(PagingParameters paging);
        Task<PagedResult<ProgressTrendDto>> GetProgressTrendsAsync(PagingParameters paging);
        Task<byte[]> ExportStudentsAsCsvAsync();
    }

}
