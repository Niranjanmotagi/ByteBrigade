namespace HackathonBackend.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        // Human-readable order number (e.g. ORD-20260525-1)
        public string OrderNumber { get; set; } = "";

        public decimal TotalAmount { get; set; }      // sum of line totals (before discount)
        public decimal DiscountAmount { get; set; }   // applied promo discount
        public decimal FinalAmount { get; set; }      // TotalAmount - DiscountAmount

        public int? PromotionId { get; set; }         // applied promo (optional)

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? EstimatedDeliveryDate { get; set; }

        // "Pending Validation" -> "Approved" / "Rejected" -> "Delivered"
        public string Status { get; set; } = "Pending Validation";

        // Optional rejection reason set by admin when rejecting
        public string? RejectionReason { get; set; }

        // Delivery details captured at checkout
        public string DeliveryAddress { get; set; } = "";
        public string DeliveryPhone { get; set; } = "";
        public string DeliveryNotes { get; set; } = "";

        // Filename of uploaded prescription (optional)
        public string? PrescriptionFile { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}
