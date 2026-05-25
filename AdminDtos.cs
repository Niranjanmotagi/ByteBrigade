namespace HackathonBackend.Dtos
{
    // ===== Admin =====
    // Documents the contracts of admin dashboard endpoints.

    public class AdminDashboardDto
    {
        public int TotalMedicines { get; set; }
        public int TotalOrders { get; set; }
        public int PendingValidations { get; set; }
        public int LowStockMedicines { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class LowStockMedicineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int StockQuantity { get; set; }
        public string Category { get; set; } = "";
    }

    public class PrescriptionReviewDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string PrescriptionFile { get; set; } = "";
        public DateTime UploadedAt { get; set; }
    }
}
