using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentProgress.API.DTOs;
using StudentProgress.API.IServices.Analytics;

namespace StudentProgress.API.Controllers.Analytics
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase

    {
        private readonly IAnalyticsService _analyticsService;
        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        [HttpGet("class-summary")]
        public async Task<IActionResult> GetClassSummary([FromQuery] PagingParameters paging)
        {
            var result = await _analyticsService.GetClassSummaryAsync(paging);
            return Ok(result);
        }

        [HttpGet("progress-trends")]
        public async Task<IActionResult> GetProgressTrends([FromQuery] PagingParameters paging)
        {
            var result = await _analyticsService.GetProgressTrendsAsync(paging);
            return Ok(result);
        }

    }
}
