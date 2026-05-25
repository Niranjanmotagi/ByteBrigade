using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HackathonBackend.Data;

namespace HackathonBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LoyaltyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoyaltyController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(idStr ?? "0");
        }

        [HttpGet("points")]
        public IActionResult GetPoints()
        {
            int userId = GetUserId();
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();
            return Ok(new { points = user.LoyaltyPoints });
        }

        [HttpGet("transactions")]
        public IActionResult GetTransactions()
        {
            int userId = GetUserId();
            var txns = _context.LoyaltyTransactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();
            return Ok(txns);
        }
    }
}
