namespace HackathonBackend.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int MedicineId { get; set; }

        public int Quantity { get; set; }

        // SRS: UnitPrice + TotalPrice (TotalPrice = UnitPrice * Quantity, persisted for invoice clarity)
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public Medicine? Medicine { get; set; }
    }
}
