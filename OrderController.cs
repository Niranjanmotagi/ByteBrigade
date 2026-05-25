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
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        // Loyalty rule: 1 point per ₹10 of FinalAmount
        private const decimal LoyaltyAmountPerPoint = 10m;

        public OrderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private int GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(idStr ?? "0");
        }

        // Customer places an order from their cart.
        // multipart/form-data: optional `prescription` file + optional `promoCode` field.
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder(
            [FromForm] IFormFile? prescription,
            [FromForm] string? promoCode)
        {
            int userId = GetUserId();

            var cart = _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Medicine)
                .ToList();

            if (!cart.Any())
                return BadRequest(new { message = "Cart is empty" });

            // Stock check
            foreach (var item in cart)
            {
                if (item.Medicine == null)
                    return BadRequest(new { message = "Invalid medicine in cart" });
                if (item.Medicine.StockQuantity < item.Quantity)
                    return BadRequest(new { message = $"Not enough stock for {item.Medicine.Name}" });
            }

            // Prescription handling
            bool needsPrescription = cart.Any(c => c.Medicine!.RequiresPrescription);
            string? savedFileName = null;

            if (needsPrescription)
            {
                if (prescription == null || prescription.Length == 0)
                    return BadRequest(new { message = "Prescription file is required for one or more items" });

                var folder = Path.Combine(_env.ContentRootPath, "wwwroot", "prescriptions");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                savedFileName = $"{Guid.NewGuid()}_{Path.GetFileName(prescription.FileName)}";
                var fullPath = Path.Combine(folder, savedFileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await prescription.CopyToAsync(stream);
            }

            // Amount calculation
            decimal totalAmount = cart.Sum(c => c.Medicine!.Price * c.Quantity);
            decimal discountAmount = 0m;
            int? promotionId = null;

            if (!string.IsNullOrWhiteSpace(promoCode))
            {
                var now = DateTime.UtcNow;
                var promo = _context.Promotions.FirstOrDefault(p =>
                    p.PromotionCode == promoCode && p.IsActive &&
                    p.StartDate <= now && p.EndDate >= now);

                if (promo == null)
                    return BadRequest(new { message = "Invalid or expired promo code" });
                if (totalAmount < promo.MinimumOrderValue)
                    return BadRequest(new { message = $"Minimum order ₹{promo.MinimumOrderValue} required for this code" });
                if (promo.UsageLimit.HasValue && promo.UsedCount >= promo.UsageLimit.Value)
                    return BadRequest(new { message = "Promo code usage limit reached" });

                discountAmount = promo.DiscountType == "Percentage"
                    ? Math.Round(totalAmount * promo.DiscountValue / 100m, 2)
                    : promo.DiscountValue;

                if (promo.MaxDiscountAmount.HasValue && discountAmount > promo.MaxDiscountAmount.Value)
                    discountAmount = promo.MaxDiscountAmount.Value;

                if (discountAmount > totalAmount) discountAmount = totalAmount;

                promo.UsedCount += 1;
                promotionId = promo.Id;
            }

            decimal finalAmount = totalAmount - discountAmount;

            // Build order
            var orderDate = DateTime.UtcNow;
            var order = new Order
            {
                UserId = userId,
                OrderNumber = $"ORD-{orderDate:yyyyMMddHHmmss}-{userId}",
                OrderDate = orderDate,
                EstimatedDeliveryDate = orderDate.AddDays(3),
                Status = "Pending",
                PrescriptionFile = savedFileName,
                PromotionId = promotionId,
                TotalAmount = totalAmount,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                Items = cart.Select(c => new OrderItem
                {
                    MedicineId = c.MedicineId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Medicine!.Price,
                    TotalPrice = c.Medicine!.Price * c.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);

            // Reduce inventory
            foreach (var item in cart)
                item.Medicine!.StockQuantity -= item.Quantity;

            // Award loyalty points (after we have order.Id, so SaveChanges first)
            _context.CartItems.RemoveRange(cart);
            _context.SaveChanges();

            int pointsEarned = (int)Math.Floor(finalAmount / LoyaltyAmountPerPoint);
            if (pointsEarned > 0)
            {
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    user.LoyaltyPoints += pointsEarned;
                    _context.LoyaltyTransactions.Add(new LoyaltyTransaction
                    {
                        UserId = userId,
                        OrderId = order.Id,
                        PointsEarned = pointsEarned,
                        PointsRedeemed = 0,
                        TransactionDate = DateTime.UtcNow,
                        Description = $"Earned for order {order.OrderNumber}"
                    });
                    _context.SaveChanges();
                }
            }

            return Ok(new
            {
                message = "Order placed",
                orderId = order.Id,
                orderNumber = order.OrderNumber,
                totalAmount = order.TotalAmount,
                discountAmount = order.DiscountAmount,
                finalAmount = order.FinalAmount,
                estimatedDeliveryDate = order.EstimatedDeliveryDate,
                pointsEarned
            });
        }

        // Customer: my orders
        [HttpGet("my")]
        public IActionResult MyOrders()
        {
            int userId = GetUserId();

            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Medicine)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return Ok(orders);
        }

        // Admin: see all orders
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult AllOrders()
        {
            var orders = _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Medicine)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return Ok(orders);
        }

        // Admin: change order status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateStatus(int id, [FromBody] string status)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            order.Status = status;
            _context.SaveChanges();

            return Ok(order);
        }
    }
}
