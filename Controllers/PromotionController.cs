using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HackathonBackend.Data;
using HackathonBackend.Models;

namespace HackathonBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PromotionController(AppDbContext context)
        {
            _context = context;
        }

        public class ValidateDto
        {
            public string Code { get; set; } = "";
            public decimal OrderAmount { get; set; }
        }

        // GET active promotions (public list)
        [HttpGet]
        public IActionResult GetActive()
        {
            var now = DateTime.UtcNow;
            var promos = _context.Promotions
                .Where(p => p.IsActive && p.StartDate <= now && p.EndDate >= now)
                .ToList();

            return Ok(promos);
        }

        // POST validate a promo code for a given order amount.
        // Returns the discount that would apply.
        [HttpPost("validate")]
        [Authorize]
        public IActionResult Validate(ValidateDto dto)
        {
            var now = DateTime.UtcNow;
            var promo = _context.Promotions.FirstOrDefault(p =>
                p.PromotionCode == dto.Code && p.IsActive &&
                p.StartDate <= now && p.EndDate >= now);

            if (promo == null)
                return BadRequest(new { message = "Invalid or expired promo code" });

            if (dto.OrderAmount < promo.MinimumOrderValue)
                return BadRequest(new { message = $"Minimum order ₹{promo.MinimumOrderValue} required" });

            if (promo.UsageLimit.HasValue && promo.UsedCount >= promo.UsageLimit.Value)
                return BadRequest(new { message = "Promo code usage limit reached" });

            decimal discount = promo.DiscountType == "Percentage"
                ? Math.Round(dto.OrderAmount * promo.DiscountValue / 100m, 2)
                : promo.DiscountValue;

            if (promo.MaxDiscountAmount.HasValue && discount > promo.MaxDiscountAmount.Value)
                discount = promo.MaxDiscountAmount.Value;

            if (discount > dto.OrderAmount) discount = dto.OrderAmount;

            return Ok(new
            {
                promotionId = promo.Id,
                code = promo.PromotionCode,
                description = promo.Description,
                discount
            });
        }

        // POST create (Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Promotion promo)
        {
            _context.Promotions.Add(promo);
            _context.SaveChanges();
            return Ok(promo);
        }
    }
}
