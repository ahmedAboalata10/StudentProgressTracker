using StudentProgress.API.DTOs.Auth;
using System.Security.Claims;

namespace StudentProgress.API.IServices.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDTO loginDto);
        Task<UserProfileDto?> GetProfileAsync(ClaimsPrincipal userClaims);
    }
}
