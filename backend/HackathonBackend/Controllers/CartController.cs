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

        public class UpdateCartQuantityDto
        {
            public int Quantity { get; set; }
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

        [HttpGet("total")]
        public IActionResult GetCartTotal()
        {
            int userId = GetUserId();

            var items = _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Medicine)
                .ToList();

            return Ok(new
            {
                totalQuantity = items.Sum(c => c.Quantity),
                totalPrice = items.Sum(c => c.TotalPrice)
            });
        }

        [HttpPost]
        public IActionResult AddToCart(AddToCartDto dto)
        {
            int userId = GetUserId();

            if (dto.Quantity <= 0)
                return BadRequest(new { message = "Quantity must be at least 1" });

            var medicine = _context.Medicines.Find(dto.MedicineId);
            if (medicine == null) return NotFound(new { message = "Medicine not found" });

            // If already in cart, just bump quantity
            var existing = _context.CartItems
                .FirstOrDefault(c => c.UserId == userId && c.MedicineId == dto.MedicineId);

            var requestedQuantity = dto.Quantity + (existing?.Quantity ?? 0);
            if (requestedQuantity > medicine.StockQuantity)
                return BadRequest(new { message = $"Only {medicine.StockQuantity} item(s) available in stock" });

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

        [HttpPut("{id}")]
        public IActionResult UpdateQuantity(int id, UpdateCartQuantityDto dto)
        {
            int userId = GetUserId();

            if (dto.Quantity <= 0)
                return BadRequest(new { message = "Quantity must be at least 1" });

            var item = _context.CartItems
                .Include(c => c.Medicine)
                .FirstOrDefault(c => c.Id == id && c.UserId == userId);

            if (item == null) return NotFound(new { message = "Cart item not found" });
            if (item.Medicine == null) return BadRequest(new { message = "Medicine not found" });
            if (dto.Quantity > item.Medicine.StockQuantity)
                return BadRequest(new { message = $"Only {item.Medicine.StockQuantity} item(s) available in stock" });

            item.Quantity = dto.Quantity;
            _context.SaveChanges();

            return Ok(new
            {
                message = "Quantity updated",
                item,
                totalPrice = item.TotalPrice
            });
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
