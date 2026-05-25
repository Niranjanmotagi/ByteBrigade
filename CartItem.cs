namespace HackathonBackend.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MedicineId { get; set; }

        public int Quantity { get; set; }

        // Navigation (not required, but handy for joins)
        public Medicine? Medicine { get; set; }
    }
}
