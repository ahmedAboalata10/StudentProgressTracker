namespace StudentProgress.API.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
