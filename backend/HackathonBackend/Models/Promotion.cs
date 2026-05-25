namespace HackathonBackend.Models
{
    public class Promotion
    {
        public int Id { get; set; }

        public string PromotionCode { get; set; } = ""; // unique
        public string Description { get; set; } = "";

        // "Percentage" or "FixedAmount"
        public string DiscountType { get; set; } = "Percentage";

        public decimal DiscountValue { get; set; }      // 10 = 10% or ₹10
        public decimal MinimumOrderValue { get; set; }  // 0 = no minimum
        public decimal? MaxDiscountAmount { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddMonths(1);

        public bool IsActive { get; set; } = true;
        public int? UsageLimit { get; set; }
        public int UsedCount { get; set; } = 0;
    }
}
