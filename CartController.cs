using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HackathonBackend.Data;
using HackathonBackend.Models;

namespace HackathonBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(idStr ?? "0");
        }

        public class AddToCartDto
        {
            public int MedicineId { get; set; }
            public int Quantity { get; set; } = 1;
        }

        // GET the current user's cart with medicine details
        [HttpGet]
        public IActionResult GetMyCart()
        {
            int userId = GetUserId();

            var items = _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Medicine)
                .ToList();

            return Ok(items);
        }

        [HttpPost]
        public IActionResult AddToCart(AddToCartDto dto)
        {
            int userId = GetUserId();

            var medicine = _context.Medicines.Find(dto.MedicineId);
            if (medicine == null) return NotFound(new { message = "Medicine not found" });

            // If already in cart, just bump quantity
            var existing = _context.CartItems
                .FirstOrDefault(c => c.UserId == userId && c.MedicineId == dto.MedicineId);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    UserId = userId,
                    MedicineId = dto.MedicineId,
                    Quantity = dto.Quantity
                });
            }

            _context.SaveChanges();
            return Ok(new { message = "Added to cart" });
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveFromCart(int id)
        {
            int userId = GetUserId();

            var item = _context.CartItems
                .FirstOrDefault(c => c.Id == id && c.UserId == userId);

            if (item == null) return NotFound();

            _context.CartItems.Remove(item);
            _context.SaveChanges();
            return Ok(new { message = "Removed" });
        }
    }
}
