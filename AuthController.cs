using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HackathonBackend.Data;
using HackathonBackend.Models;

namespace HackathonBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // DTOs (kept inline to keep things simple)
        public class RegisterDto
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
            public string Role { get; set; } = "Customer";

            // SRS profile fields (optional in form)
            public string Email { get; set; } = "";
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string PhoneNumber { get; set; } = "";
        }

        public class LoginDto
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
                return BadRequest(new { message = "Username already taken" });

            if (!string.IsNullOrWhiteSpace(dto.Email) &&
                _context.Users.Any(u => u.Email == dto.Email))
                return BadRequest(new { message = "Email already registered" });

            // Force role to Customer unless explicitly Admin (basic guard)
            var role = dto.Role == "Admin" ? "Admin" : "Customer";

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password, // plain text for hackathon simplicity
                Role = role,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                LoyaltyPoints = 0
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Registered successfully", userId = user.Id });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(
                u => u.Username == dto.Username && u.Password == dto.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("THIS_IS_MY_SUPER_SECRET_JWT_KEY_123456789");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                username = user.Username,
                role = user.Role,
                userId = user.Id
            });
        }
    }
}
