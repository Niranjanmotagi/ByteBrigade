namespace HackathonBackend.Dtos
{
    // ===== Promotion =====
    // Documents the contracts of /api/Promotion endpoints.

    public class PromotionDto
    {
        public int Id { get; set; }
        public string PromotionCode { get; set; } = "";
        public string Description { get; set; } = "";
        public string DiscountType { get; set; } = "Percentage"; // Percentage | FixedAmount
        public decimal DiscountValue { get; set; }
        public decimal MinimumOrderValue { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int? UsageLimit { get; set; }
        public int UsedCount { get; set; }
    }

    public class CreatePromotionDto
    {
        public string PromotionCode { get; set; } = "";
        public string Description { get; set; } = "";
        public string DiscountType { get; set; } = "Percentage";
        public decimal DiscountValue { get; set; }
        public decimal MinimumOrderValue { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int? UsageLimit { get; set; }
    }

    public class ValidatePromotionDto
    {
        public string Code { get; set; } = "";
        public decimal OrderAmount { get; set; }
    }

    public class ValidatePromotionResponseDto
    {
        public int PromotionId { get; set; }
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Discount { get; set; }
    }
}
