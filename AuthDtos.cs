namespace HackathonBackend.Dtos
{
    // ===== Auth =====
    // These DTOs document the request / response shape of /api/Auth endpoints.
    // The live controller uses its own inline DTOs (see AuthController.cs).

    public class RegisterDto
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "Customer";

        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
    }

    public class RegisterResponseDto
    {
        public string Message { get; set; } = "";
        public int UserId { get; set; }
    }

    public class LoginDto
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = "";
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";
        public int UserId { get; set; }
    }
}
