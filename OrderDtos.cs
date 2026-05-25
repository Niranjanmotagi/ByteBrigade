using Microsoft.AspNetCore.Http;

namespace HackathonBackend.Dtos
{
    // ===== Order =====
    // Documents the contracts of /api/Order endpoints.
    // The live controller uses PlaceOrderRequest from Models for multipart binding.

    public class PlaceOrderDto
    {
        public IFormFile? Prescription { get; set; }
        public string? PromoCode { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Notes { get; set; }
    }

    public class PlaceOrderResponseDto
    {
        public string Message { get; set; } = "";
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = "";
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public int PointsEarned { get; set; }
    }

    public class OrderItemDto
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public string Status { get; set; } = "";
        public string? RejectionReason { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string DeliveryAddress { get; set; } = "";
        public string DeliveryPhone { get; set; } = "";
        public string DeliveryNotes { get; set; } = "";
        public string? PrescriptionFile { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = "";
        public string? RejectionReason { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
    }

    public class ApproveOrderDto
    {
        public DateTime EstimatedDeliveryDate { get; set; }
    }

    public class RejectOrderDto
    {
        public string Reason { get; set; } = "";
    }
}
