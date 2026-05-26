namespace HackathonBackend.Dtos
{
    // ===== Cart =====
    // Documents the contracts of /api/Cart endpoints.

    public class AddToCartDto
    {
        public int MedicineId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartQuantityDto
    {
        public int Quantity { get; set; }
    }

    public class CartItemDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CartTotalDto
    {
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
