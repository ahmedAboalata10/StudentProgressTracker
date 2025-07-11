using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentProgress.API.DTOs.Auth;
using StudentProgress.API.IServices.Auth;
using StudentProgress.API.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentProgress.API.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var profile = await _authService.GetProfileAsync(User);
            return profile == null ? NotFound() : Ok(profile);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-data")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("Only admins can see this.");
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("teacher-data")]
        public IActionResult TeacherAndAdminEndpoint()
        {
            return Ok("Teachers and admins can see this.");
        }


    }


}
